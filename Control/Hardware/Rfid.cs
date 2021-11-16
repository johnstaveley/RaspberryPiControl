using Iot.Device.Card.Mifare;
using Iot.Device.Card.Ultralight;
using Iot.Device.Mfrc522;
using Iot.Device.Ndef;
using Iot.Device.Rfid;
using System;
using System.Text;

namespace Control.Hardware
{
    /// <summary>
    /// Taken from https://github.com/dotnet/iot/blob/main/src/devices/Mfrc522/samples/Program.cs
    /// </summary>
    public static class Rfid
    {
        public static void Process(Data106kbpsTypeA card, MfRc522 mfrc522)
        {
            if (UltralightCard.IsUltralightCard(card.Atqa, card.Sak))
            {
                Console.WriteLine("Ultralight card detected, running various tests.");
                ProcessUltralight();
            }
            else
            {
                Console.WriteLine("Mifare card detected, dumping the memory.");
                ProcessMifare();
            }

            void ProcessMifare()
            {
                var mifare = new MifareCard(mfrc522!, 0);
                mifare.SerialNumber = card.NfcId;
                mifare.Capacity = MifareCardCapacity.Mifare1K;
                mifare.KeyA = MifareCard.DefaultKeyA.ToArray();
                mifare.KeyB = MifareCard.DefaultKeyB.ToArray();
                int ret;

                for (byte block = 0; block < 64; block++)
                {
                    mifare.BlockNumber = block;
                    mifare.Command = MifareCardCommand.AuthenticationB;
                    ret = mifare.RunMifareCardCommand();
                    if (ret < 0)
                    {
                        // If you have an authentication error, you have to deselect and reselect the card again and retry
                        // Those next lines shows how to try to authenticate with other known default keys
                        mifare.ReselectCard();
                        // Try the other key
                        mifare.KeyA = MifareCard.DefaultKeyA.ToArray();
                        mifare.Command = MifareCardCommand.AuthenticationA;
                        ret = mifare.RunMifareCardCommand();
                        if (ret < 0)
                        {
                            mifare.ReselectCard();
                            mifare.KeyA = MifareCard.DefaultBlocksNdefKeyA.ToArray();
                            mifare.Command = MifareCardCommand.AuthenticationA;
                            ret = mifare.RunMifareCardCommand();
                            if (ret < 0)
                            {
                                mifare.ReselectCard();
                                mifare.KeyA = MifareCard.DefaultFirstBlockNdefKeyA.ToArray();
                                mifare.Command = MifareCardCommand.AuthenticationA;
                                ret = mifare.RunMifareCardCommand();
                                if (ret < 0)
                                {
                                    mifare.ReselectCard();
                                    Console.WriteLine($"Error reading bloc: {block}");
                                }
                            }
                        }
                    }

                    if (ret >= 0)
                    {
                        mifare.BlockNumber = block;
                        mifare.Command = MifareCardCommand.Read16Bytes;
                        ret = mifare.RunMifareCardCommand();
                        if (ret >= 0)
                        {
                            if (mifare.Data is object)
                            {
                                Console.WriteLine($"Bloc: {block}, Data: {BitConverter.ToString(mifare.Data)}");
                            }
                        }
                        else
                        {
                            mifare.ReselectCard();
                            Console.WriteLine($"Error reading bloc: {block}");
                        }

                        if (block % 4 == 3)
                        {
                            if (mifare.Data != null)
                            {
                                // Check what are the permissions
                                for (byte j = 3; j > 0; j--)
                                {
                                    var access = mifare.BlockAccess((byte)(block - j), mifare.Data);
                                    Console.WriteLine($"Bloc: {block - j}, Access: {access}");
                                }

                                var sector = mifare.SectorTailerAccess(block, mifare.Data);
                                Console.WriteLine($"Bloc: {block}, Access: {sector}");
                            }
                            else
                            {
                                Console.WriteLine("Can't check any sector bloc");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Authentication error");
                    }
                }
            }

            void ProcessUltralight()
            {
                var ultralight = new UltralightCard(mfrc522!, 0);
                ultralight.SerialNumber = card.NfcId;
                Console.WriteLine($"Type: {ultralight.UltralightCardType}, Ndef capacity: {ultralight.NdefCapacity}");

                var version = ultralight.GetVersion();
                if ((version != null) && (version.Length > 0))
                {
                    Console.WriteLine("Get Version details: ");
                    for (int i = 0; i < version.Length; i++)
                    {
                        Console.Write($"{version[i]:X2} ");
                    }

                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Can't read the version.");
                }

                var sign = ultralight.GetSignature();
                if ((sign != null) && (sign.Length > 0))
                {
                    Console.WriteLine("Signature: ");
                    for (int i = 0; i < sign.Length; i++)
                    {
                        Console.Write($"{sign[i]:X2} ");
                    }

                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Can't read the signature.");
                }

                // The ReadFast feature can be used as well, note that the MFRC522 has a very limited FIFO
                // So maximum 9 pages can be read as once.
                Console.WriteLine("Fast read example:");
                var buff = ultralight.ReadFast(0, 8);
                if (buff != null)
                {
                    for (int i = 0; i < buff.Length / 4; i++)
                    {
                        Console.WriteLine($"  Block {i} - {buff[i * 4]:X2} {buff[i * 4 + 1]:X2} {buff[i * 4 + 2]:X2} {buff[i * 4 + 3]:X2}");
                    }
                }

                Console.WriteLine("Dump of all the card:");
                for (int block = 0; block < ultralight.NumberBlocks; block++)
                {
                    ultralight.BlockNumber = (byte)block; // Safe cast, can't be more than 255
                    ultralight.Command = UltralightCommand.Read16Bytes;
                    var resI = ultralight.RunUltralightCommand();
                    if (resI > 0)
                    {
                        Console.Write($"  Block: {ultralight.BlockNumber:X2} - ");
                        for (int i = 0; i < 4; i++)
                        {
                            Console.Write($"{ultralight.Data![i]:X2} ");
                        }

                        var isReadOnly = ultralight.IsPageReadOnly(ultralight.BlockNumber);
                        Console.Write($"- Read only: {isReadOnly} ");

                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Can't read card");
                        break;
                    }
                }

                Console.WriteLine("Configuration of the card");
                // Get the Configuration
                bool res = ultralight.TryGetConfiguration(out Configuration configuration);
                if (res)
                {
                    Console.WriteLine("  Mirror:");
                    Console.WriteLine($"    {configuration.Mirror.MirrorType}, page: {configuration.Mirror.Page}, position: {configuration.Mirror.Position}");
                    Console.WriteLine("  Authentication:");
                    Console.WriteLine($"    Page req auth: {configuration.Authentication.AuthenticationPageRequirement}, Is auth req for read and write: {configuration.Authentication.IsReadWriteAuthenticationRequired}");
                    Console.WriteLine($"    Is write lock: {configuration.Authentication.IsWritingLocked}, Max num tries: {configuration.Authentication.MaximumNumberOfPossibleTries}");
                    Console.WriteLine("  NFC Counter:");
                    Console.WriteLine($"    Enabled: {configuration.NfcCounter.IsEnabled}, Password protected: {configuration.NfcCounter.IsPasswordProtected}");
                    Console.WriteLine($"  Is strong modulation: {configuration.IsStrongModulation}");
                }
                else
                {
                    Console.WriteLine("Error getting the configuration");
                }

                NdefMessage message;
                res = ultralight.TryReadNdefMessage(out message);
                if (res && message.Length != 0)
                {
                    foreach (var record in message.Records)
                    {
                        Console.WriteLine($"Record length: {record.Length}");
                        if (TextRecord.IsTextRecord(record))
                        {
                            var text = new TextRecord(record);
                            Console.WriteLine(text.Text);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No NDEF message in this ");
                }

                res = ultralight.IsFormattedNdef();
                if (!res)
                {
                    Console.WriteLine("Card is not NDEF formated, we will try to format it");
                    res = ultralight.FormatNdef();
                    if (!res)
                    {
                        Console.WriteLine("Impossible to format in NDEF, we will still try to write NDEF content.");
                    }
                    else
                    {
                        res = ultralight.IsFormattedNdef();
                        if (res)
                        {
                            Console.WriteLine("Formating successful");
                        }
                        else
                        {
                            Console.WriteLine("Card is not NDEF formated.");
                        }
                    }
                }

                NdefMessage newMessage = new NdefMessage();
                newMessage.Records.Add(new TextRecord("I ❤ .NET IoT", "en", Encoding.UTF8));
                res = ultralight.WriteNdefMessage(newMessage);
                if (res)
                {
                    Console.WriteLine("NDEF data successfully written on the card.");
                }
                else
                {
                    Console.WriteLine("Error writing NDEF data on card");
                }
            }
        }
    }
}

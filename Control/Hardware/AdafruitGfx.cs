//namespace Control.Hardware
//{
//    public class AdafruitGfx : LedBackPack
//    {
//        protected byte Rotation { get; set; }
//        protected byte Width { get; }
//        protected byte Height { get; }

//        public AdafruitGfx(byte width, byte height, int address) : base(address)
//        {
//            Width = width;
//            Height = height;
//            Rotation = 0;
//        }

//        /**************************************************************************/
///*!
//   @brief      Draw a PROGMEM-resident 1-bit image at the specified (x,y)
//   position, using the specified foreground color (unset bits are transparent).
//    @param    x   Top left corner x coordinate
//    @param    y   Top left corner y coordinate
//    @param    bitmap  byte array with monochrome bitmap
//    @param    w   Width of bitmap in pixels
//    @param    h   Height of bitmap in pixels
//    @param    color 16-bit 5-6-5 Color to draw with
//*/
///**************************************************************************/
//        public void drawBitmap(short x, short y, byte[] bitmap, short w, short h, short color) {

//            short byteWidth = (short) ((w + 7) / 8); // Bitmap scanline pad = whole byte
//            byte byte1 = 0;

//            for (short j = 0; j < h; j++, y++) {
//                for (short i = 0; i < w; i++) {
//                    if (i & 7)
//                        byte1 <<= 1;
//                    else
//                        byte1 = pgm_read_byte(&bitmap[j * byteWidth + i / 8]);
//                    if (byte1 & 0x80)
//                        WritePixel(x + i, y, color);
//                }
//            }
//        }
//    }

    
///**************************************************************************/
///*!
//   @brief    Write a pixel, overwrite in subclasses if startWrite is defined!
//    @param   x   x coordinate
//    @param   y   y coordinate
//   @param    color 16-bit 5-6-5 Color to fill with
//*/
///**************************************************************************/
//    protected void WritePixel(short x, short y, short color) {
//        DrawPixel(x, y, color);
//    }

    
///**************************************************************************/
///*!
//    @brief  Draw a pixel to the canvas framebuffer
//    @param  x     x coordinate
//    @param  y     y coordinate
//    @param  color Binary (on or off) color to fill with
//*/
///**************************************************************************/
//    protected void DrawPixel(short x, short y, short color) {
//    if (buffer) {
//    if ((x < 0) || (y < 0) || (x >= _width) || (y >= _height))
//    return;

//    short t;
//    switch (rotation) {
//    case 1:
//    t = x;
//    x = WIDTH - 1 - y;
//    y = t;
//    break;
//    case 2:
//    x = WIDTH - 1 - x;
//    y = HEIGHT - 1 - y;
//    break;
//    case 3:
//    t = x;
//    x = y;
//    y = HEIGHT - 1 - t;
//    break;
//}

//byte *ptr = &buffer[(x / 8) + y * ((WIDTH + 7) / 8)];
//if (color)
//    *ptr |= 0x80 >> (x & 7);
//else
//    *ptr &= ~(0x80 >> (x & 7));
//}
//}

//}

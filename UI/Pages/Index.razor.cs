using Common;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.Services;

namespace UI.Pages
{
    public partial class Index
    {
        public string Message { get; set; }
        public string MessageClass { get; set; }
        public string OnOffSelected { get; set; }
        public List<string> OnOff { get; set; }
        public List<string> Operations { get; set; }
        public List<int> Pwms { get; set; }
        public List<string> Outputs { get; set; }
        public List<string> Relays { get; set; }
        public bool SelectedOnOff { get; set; }
        public string SelectedOperation = "(Please Select)";
        public int SelectedOutput { get; set; } = 1;
        public int SelectedPwm { get; set; }
        public string SelectedRelay { get; set; }
        public string SelectedText { get; set; }
        public double SelectedValue { get; set; }
        public bool HideOnOff { get; set; } = false;
        public bool HideOutputs { get; set; } = false;
        public bool HidePwm { get; set; } = false;
        public bool HideRelay { get; set; } = false;
        public bool HideText { get; set; } = false;
        public bool HideValue { get; set; } = false;

        [Inject]
        protected IThingService ThingService { get; set; }

        protected override Task OnInitializedAsync()
        {
            OnOff = new List<string> { "Off", "On" };
            Operations = new List<string>();
            Operations.AddRange(Consts.OperationList);
            Operations.Insert(0, "(Please Select)");
            Outputs = new List<string> { "1", "2", "3", "All" };
            Pwms = Enumerable.Range(0, 16).ToList();
            Relays = new List<string> { "1", "2", "3", "4", "All" };
            SelectedOperation = "(Please Select)";
            SelectedRelay = "1";
            SelectedOnOff = false;
            ChangeDisplay();
            return base.OnInitializedAsync();
        }

        public async Task SendMessage()
        {
            try
            {
                Message = "";
                if (string.IsNullOrEmpty(SelectedOperation) || SelectedOperation == "(Please Select)")
                {
                    Message = "Please select an operation";
                    return;
                }
                int number = 0;
                double value = 0;
                string text = "";
                switch (SelectedOperation)
                {
                    case Consts.Operations.GetRelay:
                        if (SelectedRelay == "All") SelectedRelay = "-1";
                        number = int.Parse(SelectedRelay);
                        break;
                    case Consts.Operations.SetOutput:
                        number = SelectedOutput;
                        value = SelectedOnOff ? 1 : 0;
                        break;
                    case Consts.Operations.SetText:
                        number = 2; // Row
                        value = 0; // Offset
                        text = SelectedText;
                        break;
                    case Consts.Operations.SetRelay:
                        if (SelectedRelay == "All") SelectedRelay = "-1";
                        number = int.Parse(SelectedRelay);
                        value = SelectedOnOff ? 1 : 0;
                        break;
                    case Consts.Operations.SetPwm:
                        number = SelectedPwm;
                        value = SelectedValue / 100;
                        break;
                }

                var result = await ThingService.SendMessage(SelectedOperation, number, value, text);
                MessageClass = "alert-info";
                if (!result.Success)
                {
                    MessageClass = "alert-danger";
                }
                Message = result.Message;
            }
            catch (Exception exception)
            {
                Message = "An exception occurred: " + exception.Message;
                MessageClass = "alert-danger";
            }
        }

        private void ChangeDisplay(string operation = null)
        {
            HidePwm = true;
            HideRelay = true;
            HideOnOff = true;
            HideOutputs = true;
            HideText = true;
            HideValue = true;
            switch (operation)
            {
                case Consts.Operations.GetRelay:
                    HideRelay = false;
                    break;
                case Consts.Operations.SetOutput:
                    HideOnOff = false;
                    HideOutputs = false;
                    break;
                case Consts.Operations.SetPwm:
                    HidePwm = false;
                    HideValue = false;
                    break;
                case Consts.Operations.SetRelay:
                    HideRelay = false;
                    HideOnOff = false;
                    break;
                case (Consts.Operations.SetText):
                    HideText = false;
                    break;
                default:
                    // Do nothing
                    break;
            }
        }
        public void ChangeOnOff(ChangeEventArgs e)
        {
            SelectedOnOff = e.Value.ToString() == "On";
        }
        public void ChangeOperation(ChangeEventArgs e)
        {
            SelectedOperation = e.Value.ToString();
            ChangeDisplay(SelectedOperation);
        }
        public void ChangeOutput(ChangeEventArgs e)
        {
            if (e.Value.ToString() == "All") {
                SelectedOutput = -1;
            } else {
                SelectedOutput = int.Parse(e.Value.ToString());
            }
        }
        public void ChangePwm(ChangeEventArgs e)
        {
            SelectedPwm = int.Parse(e.Value.ToString());
        }
        public void ChangeRelay(ChangeEventArgs e)
        {
            SelectedRelay = e.Value.ToString();
        }
        public void ChangeText(ChangeEventArgs e)
        {
            SelectedText = e.Value.ToString();
        }
        public void ChangeValue(ChangeEventArgs e)
        {
            SelectedValue = int.Parse(e.Value.ToString());
        }
    }
}

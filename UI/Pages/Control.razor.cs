﻿using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.Model;
using UI.Services;

namespace UI.Pages
{
    [Authorize]
    public partial class Control
    {
        public string EventMessage { get; set; }
        public string EventMessageClass { get; set; }
        public string Message { get; set; }
        public string MessageClass { get; set; }
        public string OnOffSelected { get; set; }
        public List<string> OnOff { get; set; }
        public List<string> Operations { get; set; }
        public List<int> Pwms { get; set; }
        public List<string> Outputs { get; set; }
        public List<string> Channels { get; set; }
        public List<string> TextDisplayTypes { get; set; }
        public bool SelectedOnOff { get; set; }
        public string SelectedOperation = "(Please Select)";
        public int SelectedOutput { get; set; } = 1;
        public int SelectedPwm { get; set; }
        public string SelectedChannel { get; set; }
        public string SelectedText { get; set; }
        public int SelectedTextDisplay { get; set; }
        public double SelectedValue { get; set; }
        public bool HideOnOff { get; set; } = false;
        public bool HideOutputs { get; set; } = false;
        public bool HidePwm { get; set; } = false;
        public bool HideChannel { get; set; } = false;
        public bool HideText { get; set; } = false;
        public bool HideTextDisplay { get; set; } = false;
        public bool HideValue { get; set; } = false;
        [Inject]
        protected IIotHubService IotHubService { get; set; }
        [Inject]
        protected IThingService ThingService { get; set; }

        protected override Task OnInitializedAsync()
        {
            OnOff = new List<string> { "Off", "On" };
            Operations = new List<string>();
            Operations.AddRange(Consts.OperationList);
            Operations.Insert(0, "(Please Select)");
            Outputs = new List<string> { "1", "2", "3", "All", "Cycle" };
            Pwms = Enumerable.Range(0, 16).ToList();
            Channels = new List<string> { "1", "2", "3", "4", "All" };
            SelectedOperation = "(Please Select)";
            SelectedChannel = "1";
            SelectedOnOff = false;
            TextDisplayTypes = new List<string> {"Top", "Bottom", "Demo"};
            EventMessage = "None";
            ChangeDisplay();
            IotHubService.OnEventReceived +=  (sender, args) => { 
                EventReceived((DeviceEventArgs) args); 
                InvokeAsync(StateHasChanged);
            };
            return base.OnInitializedAsync();
        }

        private void EventReceived(DeviceEventArgs eventArgs)
        {
            EventMessageClass = "alert-info";
            EventMessage = $"{eventArgs.EventDate:dd/MM/yyyy HH:mm:ss} {eventArgs.Method}: {eventArgs.Message}";            
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
                    case Consts.Operations.GetAnalogue:
                        if (SelectedChannel == "All") SelectedChannel = "0";
                        number = int.Parse(SelectedChannel);
                        break;
                    case Consts.Operations.GetRelay:
                        if (SelectedChannel == "All") SelectedChannel = "-1";
                        number = int.Parse(SelectedChannel);
                        break;
                    case Consts.Operations.SetOutput:
                        number = SelectedOutput;
                        value = SelectedOnOff ? 1 : 0;
                        break;
                    case Consts.Operations.SetText:
                        number = SelectedTextDisplay; // Row number or demo
                        value = 0; // Offset
                        text = SelectedText;
                        break;
                    case Consts.Operations.SetRelay:
                        if (SelectedChannel == "All") SelectedChannel = "-1";
                        number = int.Parse(SelectedChannel);
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
            HideChannel = true;
            HideOnOff = true;
            HideOutputs = true;
            HideText = true;
            HideTextDisplay = true;
            HideValue = true;
            switch (operation)
            {
                case Consts.Operations.GetAnalogue:
                    HideChannel = false;
                    break;
                case Consts.Operations.GetRelay:
                    HideChannel = false;
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
                    HideChannel = false;
                    HideOnOff = false;
                    break;
                case (Consts.Operations.SetText):
                    HideText = false;
                    HideTextDisplay = false;
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
            switch (e.Value.ToString())
            {
                case "All":
                    SelectedOutput = -1;
                    break;
                case "Cycle":
                    SelectedOutput = -2;
                    break;
                default:
                    SelectedOutput = int.Parse(e.Value.ToString());
                    break;
            }
        }
        public void ChangePwm(ChangeEventArgs e)
        {
            SelectedPwm = int.Parse(e.Value.ToString());
        }
        public void ChangeChannel(ChangeEventArgs e)
        {
            SelectedChannel = e.Value.ToString();
        }
        public void ChangeText(ChangeEventArgs e)
        {
            SelectedText = e.Value.ToString();
        }
        public void ChangeTextDisplay(ChangeEventArgs e)
        {
            switch (e.Value.ToString())
            {
                case "Top":
                    SelectedTextDisplay = 1;
                    break;
                case "Bottom":
                    SelectedTextDisplay = 2;
                    break;
                case "Demo":
                    SelectedTextDisplay = -1;
                    break;
            }
        }
        public void ChangeValue(ChangeEventArgs e)
        {
            SelectedValue = int.Parse(e.Value.ToString());
        }
    }
}

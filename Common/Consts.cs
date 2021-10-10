namespace Common
{
    public static class Consts
    {
        public const string MethodName = "ControlAction";

        public static class Operations
        {
            public const string GetAnalogue = "GetAnalogue";
            public const string GetInput = "GetInput";
            public const string GetRelay = "GetRelay";
            public const string PlaySound = "PlaySound";
            public const string SetOutput = "SetOutput";
            public const string SetPwm = "SetPwm";
            public const string SetRelay = "SetRelay";
            public const string SetText = "SetText";
        }

        public static class Events
        {
            public const string Button = "Button";
            public const string StartUp = "StartUp";
            public const string IsAlive = "IsAwake";
        }

        public static string[] OperationList = new[] { 
            Operations.GetInput, 
            Operations.GetRelay, 
            Operations.PlaySound, 
            Operations.SetOutput, 
            Operations.SetPwm, 
            Operations.SetRelay, 
            Operations.SetText
        };
    }
}

namespace Common
{
    public static class Consts
    {
        public const string MethodName = "ControlAction";

        public static class Operations
        {
            public const string SetOutput = "SetOutput";
            public const string SetPwm = "SetPwm";
            public const string SetRelay = "SetRelay";
            public const string SetText = "SetText";
        }

        public static string[] OperationList = new[] { Operations.SetOutput, Operations.SetPwm, Operations.SetRelay, Operations.SetText };
    }
}

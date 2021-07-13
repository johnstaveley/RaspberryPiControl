namespace Control.Model
{
    public class ControlAction
    {
        /// <summary>
        /// The name of the hardware to control
        /// </summary>
        public string Method { get; set; }
        public double Number { get; set; }
        public double Value { get; set; }
        public string Message { get; set; }
    }
}

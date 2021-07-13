namespace Control.Model
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public ResponseModel()
        {
            Success = false;
            Message = "";
        }
    }
}

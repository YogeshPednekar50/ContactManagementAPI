namespace ContactMgmtAPI.Model.Response
{
    public class BaseResponse
    {
        public object Result { get; set; }

        public bool Success { get; set; }

        public int StatusCode { get; set; }

        public List<string> Errors { get; set; }

    }
}

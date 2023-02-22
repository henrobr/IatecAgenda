namespace api.Models.Response
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(int status, string message, T data, List<string> errors = null)
        {
            Status = status;
            Message = message;
            Data = data;
            Errors = errors;
        }
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
    } 
    
    
}

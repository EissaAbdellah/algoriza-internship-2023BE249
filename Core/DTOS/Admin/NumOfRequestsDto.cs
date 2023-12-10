namespace Core.DTOS.Admin
{
    public class NumOfRequestsDto
    {

        public int NumOfRequests { get; set; }
        public int NumOfPendingRequests { get; set; }
        public int NumOfCompletedRequests { get; set; }
        public int NumOfCancelledRequests { get; set; }

    }
}

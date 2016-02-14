namespace CodeApes.Templateer
{
    public class BackendStatus
    {
        private bool isRunning = true;
        
        public bool IsRunning 
        { 
            get { return isRunning; } 
        }
        
        public void RequestShutdown()
        {
            isRunning = false;
        }
    }
}
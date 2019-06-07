namespace QiniuSdk
{
    public interface IAuth
    {
        string CreateDownloadToken(string url);
        string CreateManageToken(string url);
        string CreateManageToken(string url, byte[] body);
        string CreateStreamManageToken(string data);
        string CreateStreamPublishToken(string path);
        string CreateUploadToken(string jsonStr);
    }
}
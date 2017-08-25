using VINCapture.UploadImage.View;

namespace VINCapture.UploadImage.Models
{
    public class DealerUser
    {
        public int DealerId { get; set; }
        public string DealerName { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string HeaderOverlayUrl { get; set; }
        public string FooterOverlayUrl { get; set; }
        public RoleType Role { get; set; }
    }
}
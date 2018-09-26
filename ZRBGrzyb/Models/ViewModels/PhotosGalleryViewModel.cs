using System.Collections.Generic;

namespace ZRBGrzyb.Models.ViewModels {

    public class PhotosGalleryViewModel {

        public IEnumerable<Photo> Photos { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}

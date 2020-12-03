using System.Collections.Generic;
using AdminProject.Services.Models;

namespace AdminProject.Models
{
    public class ProductDetailDto
    {
        public ProductDetailViewDto Product { get; set; }
        public List<MeasureListDto> Measures { get; set; }
        public List<PictureItemDto> Pictures { get; set; }
        public List<ProductCommentDto> Comments { get; set; }
        public List<CategoryLinkDto> Categories { get; set; }
    }
}
﻿using API.DTO;

namespace API.Data
{
    public class Advert
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public string Title { get; set; }
        public List<AdvertBundleDTO> AdvertBundle { get; set; }
    }
}

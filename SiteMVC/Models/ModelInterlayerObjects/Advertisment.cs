﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.Models.ModelInterlayerObjects
{
    public class Advertisment
    {
        public Advertisment(viewAdvertisment entity)
        {
            this.Id = entity.Id;
            this.Text = entity.text;
            this.CreateDate = entity.createDate;
            this.ModifyDate = entity.modifyDate;
            this.Searchresult_id = entity.searchresult_id;
            this.Link = entity.link;
            this.SiteName = entity.siteName;
            this.IsSpecial = entity.isSpecial;

            this.Price = entity.Price;
            this.Address1 = entity.Address1;

            this.Phones = entity.AdvertismentPhones.Select(p => new Phone(p)).ToList();
            this.Photos = entity.AdvertismentsPhotos.Select(p => new Photo(p)).ToList();

            this.Comments = entity.AdvertismentComments.Select(c => new Comment(c)).ToList();
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDateFormated 
        {
            get
            {
                return CreateDate.ToString("g");
            }
        }
        public DateTime ModifyDate { get; set; }
        public int? Searchresult_id { get; set; }
        public string Link { get; set; }
        public string SiteName { get; set; }
        public bool IsSpecial { get; set; }

        public decimal? Price { get; set; }
        public string Address1 { get; set; }

        public List<Phone> Phones { get; set; }
        public List<Photo> Photos { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
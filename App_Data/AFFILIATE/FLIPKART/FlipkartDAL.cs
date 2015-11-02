using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using com.fokatdeals;
namespace com.flipkart
{
    public class FlipkartDAL : CommonDAL
    {

        public FlipkartDAL()
        {
        }
        public MyCategory getCategory(List<MyCategory> categoryList, String apiName)
        {
            MyCategory cat = new MyCategory();
            foreach (MyCategory item in categoryList)
            {
                if (item.apiName == apiName)
                {
                    cat = item;
                    break;
                }
            }
            return cat;
        }

        public List<MyCategory> getCategoryList(ApiListings apiListing)
        {
            List<MyCategory> list = new List<MyCategory>();
            list.Add(apiListing.air_conditioners);
            list.Add(apiListing.air_coolers);
            list.Add(apiListing.audio_players);
            list.Add(apiListing.automotive);
            list.Add(apiListing.baby_care);
            list.Add(apiListing.bags_wallets_belts);
            list.Add(apiListing.camera_accessories);
            list.Add(apiListing.cameras);
            list.Add(apiListing.computer_components);
            list.Add(apiListing.computer_peripherals);
            list.Add(apiListing.computer_storage);
            list.Add(apiListing.desktops);
            list.Add(apiListing.e_learning);
            list.Add(apiListing.eyewear);
            list.Add(apiListing.food_nutrition);
            list.Add(apiListing.fragrances);
            list.Add(apiListing.furniture);
            list.Add(apiListing.gaming);
            list.Add(apiListing.grooming_beauty_wellness);
            list.Add(apiListing.home_and_kitchen_needs);
            list.Add(apiListing.home_appliances);
            list.Add(apiListing.home_decor_and_festive_needs);
            list.Add(apiListing.home_entertainment);
            list.Add(apiListing.home_furnishing);
            list.Add(apiListing.home_improvement_tools);
            list.Add(apiListing.household_supplies);
            list.Add(apiListing.jewellery);
            list.Add(apiListing.kids_clothing);
            list.Add(apiListing.kids_footwear);
            list.Add(apiListing.kitchen_appliances);
            list.Add(apiListing.landline_phones);
            list.Add(apiListing.laptop_accessories);
            list.Add(apiListing.laptops);
            list.Add(apiListing.luggage_travel);
            list.Add(apiListing.mens_clothing);
            list.Add(apiListing.mens_footwear);
            list.Add(apiListing.microwave_ovens);
            list.Add(apiListing.mobile_accessories);
            list.Add(apiListing.mobiles);
            list.Add(apiListing.music_movies_posters);
            list.Add(apiListing.network_components);
            list.Add(apiListing.pet_supplies);
            list.Add(apiListing.refrigerator);
            list.Add(apiListing.software);
            list.Add(apiListing.sports_fitness);
            list.Add(apiListing.stationery_office_supplies);
            list.Add(apiListing.sunglasses);
            list.Add(apiListing.tablet_accessories);
            list.Add(apiListing.tablets);
            list.Add(apiListing.televisions);
            list.Add(apiListing.toys);
            list.Add(apiListing.tv_video_accessories);
            list.Add(apiListing.video_players);
            list.Add(apiListing.washing_machine);
            list.Add(apiListing.watches);
            list.Add(apiListing.wearable_smart_devices);
            list.Add(apiListing.womens_clothing);
            list.Add(apiListing.womens_footwear);
            return list;
        }

        public void updateProducts(MyCategory category)
        {
            FlipkartProductModel exm = JsonConvert.DeserializeObject<FlipkartProductModel>(HttpGet(category.availableVariants.version.get));
            ProductModel prdModel = new ProductModel();
            ProductDAL dal = new ProductDAL();
            while (exm.nextUrl != null)
            {
                foreach (ProductInfoList item in exm.productInfoList)
                {
                    ProductAttributes att = new ProductAttributes();
                    ProductIdentifier idt = new ProductIdentifier();
                    att = item.productBaseInfo.productAttributes;
                    idt = item.productBaseInfo.productIdentifier;

                    prdModel.UniqueId = Guid.NewGuid().ToString().Substring(0, 8);
                    prdModel.PrdId = idt.productId;
                    prdModel.Name = att.title;
                    if (att.productDescription == null)
                    {
                        att.productDescription = FlipkartConstants.INVALID_DESCRIPTION;
                    }
                    prdModel.Description = att.productDescription;
                    prdModel.PrdUrl = att.productUrl;
                    prdModel.PrdRedirectUrl = att.productUrl;
                    if (att.imageUrls._275x275 == null)
                    {
                        att.imageUrls._275x275 = FlipkartConstants.DEFAULT_IMAGE;
                    }
                    prdModel.Img = att.imageUrls._275x275;
                    prdModel.Width = FlipkartConstants.DEFAULT_IMAGE_WIDTH_AND_HEIGHT_275PX;
                    prdModel.Height = FlipkartConstants.DEFAULT_IMAGE_WIDTH_AND_HEIGHT_275PX;
                    prdModel.Storeid = FlipkartConstants.FLIPKART_STORE_ID_FOKATDEALS;
                    prdModel.RegularPrice = att.maximumRetailPrice.amount + " Rs.";
                    prdModel.OfferPrice = att.sellingPrice.amount + " Rs.";
                    prdModel.SubCatId = category.apiName;
                    prdModel.CreatedOn = System.DateTime.Now + "";
                    prdModel.ExpireOn = "";
                    String status = "";
                    if (att.inStock)
                    {
                        status = "A";
                    }
                    else
                    {
                        status = "D";
                    }
                    prdModel.Status = status;
                    prdModel.Custom1 = att.productBrand;
                    prdModel.Custom2 = att.codAvailable + "";

                    int i = dal.InsertProducts(prdModel);
                }

                exm = JsonConvert.DeserializeObject<FlipkartProductModel>(HttpGet(exm.nextUrl));

            }
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using com.fokatdeals;
namespace com.flipkart
{
    public class FlipkartAPIModel
    {
        public string title { get; set; }
        public string description { get; set; }
        public ApiGroups apiGroups { get; set; }
    }
    public class V010
    {
        public string resourceName { get; set; }
        public object put { get; set; }
        public object delete { get; set; }
        public object post { get; set; }
        public string get { get; set; }
    }

    public class AvailableVariants
    {
        [JsonProperty("v0.1.0")]
        public V010 version { get; set; }
    }


    public class MyCategory
    {
        public AvailableVariants availableVariants { get; set; }
        public string apiName { get; set; }
    }

    public class ApiListings
    {
        public MyCategory bags_wallets_belts { get; set; }
        public MyCategory fragrances { get; set; }
        public MyCategory tv_video_accessories { get; set; }
        public MyCategory camera_accessories { get; set; }
        public MyCategory furniture { get; set; }
        public MyCategory sports_fitness { get; set; }
        public MyCategory mobile_accessories { get; set; }
        public MyCategory pet_supplies { get; set; }
        public MyCategory automotive { get; set; }
        public MyCategory software { get; set; }
        public MyCategory food_nutrition { get; set; }
        public MyCategory jewellery { get; set; }
        public MyCategory home_and_kitchen_needs { get; set; }
        public MyCategory televisions { get; set; }
        public MyCategory computer_storage { get; set; }
        public MyCategory mens_clothing { get; set; }
        public MyCategory stationery_office_supplies { get; set; }
        public MyCategory video_players { get; set; }
        public MyCategory tablets { get; set; }
        public MyCategory kids_footwear { get; set; }
        public MyCategory home_decor_and_festive_needs { get; set; }
        public MyCategory sunglasses { get; set; }
        public MyCategory womens_clothing { get; set; }
        public MyCategory kids_clothing { get; set; }
        public MyCategory wearable_smart_devices { get; set; }
        public MyCategory womens_footwear { get; set; }
        public MyCategory air_coolers { get; set; }
        public MyCategory music_movies_posters { get; set; }
        public MyCategory desktops { get; set; }
        public MyCategory gaming { get; set; }
        public MyCategory microwave_ovens { get; set; }
        public MyCategory laptop_accessories { get; set; }
        public MyCategory tablet_accessories { get; set; }
        public MyCategory mobiles { get; set; }
        public MyCategory grooming_beauty_wellness { get; set; }
        public MyCategory kitchen_appliances { get; set; }
        public MyCategory watches { get; set; }
        public MyCategory cameras { get; set; }
        public MyCategory home_improvement_tools { get; set; }
        public MyCategory landline_phones { get; set; }
        public MyCategory network_components { get; set; }
        public MyCategory laptops { get; set; }
        public MyCategory luggage_travel { get; set; }
        public MyCategory refrigerator { get; set; }
        public MyCategory home_entertainment { get; set; }
        public MyCategory air_conditioners { get; set; }
        public MyCategory household_supplies { get; set; }
        public MyCategory computer_peripherals { get; set; }
        public MyCategory audio_players { get; set; }
        public MyCategory home_furnishing { get; set; }
        public MyCategory baby_care { get; set; }
        public MyCategory e_learning { get; set; }
        public MyCategory toys { get; set; }
        public MyCategory home_appliances { get; set; }
        public MyCategory eyewear { get; set; }
        public MyCategory computer_components { get; set; }
        public MyCategory washing_machine { get; set; }
        public MyCategory mens_footwear { get; set; }
    }

    public class Affiliate
    {
        public string name { get; set; }
        public ApiListings apiListings { get; set; }
        public ApiListings rawDownloadListings { get; set; }
    }

    public class ApiGroups
    {
        public Affiliate affiliate { get; set; }
    }

    public class FlipkartConstants
    {
        public static String INVALID_DESCRIPTION = "No Description";
        public static String DEFAULT_IMAGE = AppConstants.DOMAIN_NAME + "images/no-product.png";
        public static String DEFAULT_IMAGE_WIDTH_AND_HEIGHT_275PX = "275px";
        public static String FLIPKART_STORE_ID_FOKATDEALS = "flipkart";

    }

}

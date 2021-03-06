using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaryamRahimiFard.Core.Utility
{
    public enum DiscountType
    {
        Percentage = 1,
        Amount = 2
    }
    public enum GeoDivisionType
    {
        Country = 0,
        State = 1,
        City = 2,
    }

    public enum StaticContents
    {
        AboutDescription = 16,
        firstImageAboutPage = 15,
        WorkingHours = 1008,
        Youtube = 29,
        Instagram = 28,
        Twitter = 27,
        Pinterest = 30,
        Facebook = 26,
        LinkedIn = 33,
        BlogImage = 1013,
        ContactInfo = 1014,
        companyServices = 3003,
        ImplementaitonService = 3005,

        Address = 5,
        Email = 6,
        Phone = 7,
        ContactUsMap = 4,
        ContactUs = 32,
        Faq = 20,
        certificate = 25,
        Gallery = 17,
        HomeUnderSliderTitle = 1037,
        HomeAbout = 2,
        HomeCounterBackGroundImage = 3,
        HomeContactUs = 15,
        HomeNewCourses = 1046,
        HomeNewArticles = 1047,

        MainTitleDescriptionHeaderFooter = 14,
        HeaderBackGroundImage = 13,
        NewsBackImage = 8,

        BlogAd = 32,
    }

    public enum StaticContentTypes
    {
        HeaderFooter = 9,
        About = 13,
        HomeCourseProperties = 5,
        HomeCounters = 18,

        HomeTopSlider = 17,
        Contact = 2,

        Guide = 9,
        Popup = 11,
        PageBanner = 12,
        HomeOurServicesUnderSlieder = 3,
    }

    public enum PaymentStatus
    {
        Unprocessed = 1,
        Failed =2,
        Succeed =3,
        Expired = 4
    }

    public enum AditionalFeatureType
    {
        Volume = 1
    }

}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital.Models.Feedback;
using Hospital.Repositories.Feedback;

namespace HospitalTests.Repositories.Feedback;

[TestClass]
public class HospitalFeedbackRepositoryTests
{
    [TestInitialize]
    public void SetUp()
    {
        DeleteData();
    }

    [TestCleanup]
    public void CleanUp()
    {
        DeleteData();
    }

    private static void DeleteData()
    {
        Directory.GetFiles("../../../Data/").ToList().ForEach(File.Delete);
    }

    private void AddData()
    {
        HospitalFeedbackRepository.Instance.Add(new HospitalFeedback(2, 1, "", 3, 4, 5));
        HospitalFeedbackRepository.Instance.Add(new HospitalFeedback(3, 1, "", 4, 4, 5));
        HospitalFeedbackRepository.Instance.Add(new HospitalFeedback(2, 1, "", 5, 4, 5));
        HospitalFeedbackRepository.Instance.Add(new HospitalFeedback(3, 1, "", 6, 4, 5));
    }

    [TestMethod]
    public void GetAverageRatingsTest()
    {
        AddData();
        var averageRatings = HospitalFeedbackRepository.Instance.GetAverageRatings();

        Assert.AreEqual(2.5, averageRatings.OverallRating);
        Assert.AreEqual(1, averageRatings.RecommendationRating);
        Assert.AreEqual(4.5, averageRatings.ServiceQuality);
        Assert.AreEqual(4, averageRatings.CleanlinessRating);
        Assert.AreEqual(5, averageRatings.PatientSatisfactionRating);
    }

    [TestMethod()]
    public void GetServiceQualityRatingFrequenciesTest()
    {
        AddData();
        var serviceQualityRatingFrequencies = HospitalFeedbackRepository.Instance.GetServiceQualityRatingFrequencies();
        Assert.AreEqual(1, serviceQualityRatingFrequencies[3]);
        Assert.AreEqual(1, serviceQualityRatingFrequencies[4]);
        Assert.AreEqual(1, serviceQualityRatingFrequencies[5]);
        Assert.AreEqual(1, serviceQualityRatingFrequencies[6]);
    }

    [TestMethod()]
    public void GetOverallRatingFrequenciesTest()
    {
        AddData();
        var overallRatingFrequencies = HospitalFeedbackRepository.Instance.GetOverallRatingFrequencies();
        Assert.AreEqual(2, overallRatingFrequencies[2]);
        Assert.AreEqual(2, overallRatingFrequencies[3]);

    }

    [TestMethod()]
    public void GetPatientSatisfactionRatingsTest()
    {
        AddData();
        var satisfactionRatingFrequencies = HospitalFeedbackRepository.Instance.GetPatientSatisfactionRatingFrequencies();
        Assert.AreEqual(4, satisfactionRatingFrequencies[5]);
    }

    [TestMethod()]
    public void GetRecommendationFrequenciesRatingsTest()
    {
        AddData();
        var recommendationRatingFrequencies = HospitalFeedbackRepository.Instance.GetRecommendationRatingFrequencies();
        Assert.AreEqual(4, recommendationRatingFrequencies[1]);
    }
}
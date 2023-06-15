using Hospital.PatientFeedback.Models;
using Hospital.PatientFeedback.Repositories;

namespace HospitalTests.Repositories.Feedback;

[TestClass]
public class DoctorFeedbackRepositoryTests
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
        DoctorFeedbackRepository.Instance.Add(new DoctorFeedback("1", 1, 1, "", 2));
        DoctorFeedbackRepository.Instance.Add(new DoctorFeedback("1", 5, 5, "", 5));
        DoctorFeedbackRepository.Instance.Add(new DoctorFeedback("2", 1, 1, "", 1));
        DoctorFeedbackRepository.Instance.Add(new DoctorFeedback("3", 2, 2, "", 2));
        DoctorFeedbackRepository.Instance.Add(new DoctorFeedback("4", 3, 3, "", 3));
        DoctorFeedbackRepository.Instance.Add(new DoctorFeedback("5", 5, 4, "", 4));
        DoctorFeedbackRepository.Instance.Add(new DoctorFeedback("6", 5, 5, "", 5));
    }

    [TestMethod]
    public void TestGetTop3Doctors()
    {
        AddData();
        var topDoctors = DoctorFeedbackRepository.Instance.GetTop3Doctors();
        Assert.AreEqual("6", topDoctors[0].DoctorId);
        Assert.AreEqual("5", topDoctors[1].DoctorId);
        Assert.AreEqual("1", topDoctors[2].DoctorId);
        Assert.AreEqual(19d / 6d, topDoctors[2].AverageRating);
    }

    [TestMethod]
    public void TestGetBottom3Doctors()
    {
        AddData();
        var bottomDoctors = DoctorFeedbackRepository.Instance.GetBottom3Doctors();
        Assert.AreEqual("4", bottomDoctors[0].DoctorId);
        Assert.AreEqual("3", bottomDoctors[1].DoctorId);
        Assert.AreEqual("2", bottomDoctors[2].DoctorId);
        Assert.AreEqual(1d, bottomDoctors[2].AverageRating);
    }

    [TestMethod]
    public void TestGetOverallRatingFrequencies()
    {
        AddData();
        var overallRatingFrequencies = DoctorFeedbackRepository.Instance.GetOverallRatingFrequencies("1");
        Assert.AreEqual(1, overallRatingFrequencies[1]);
        Assert.AreEqual(1, overallRatingFrequencies[5]);
    }

    [TestMethod]
    public void TestGetRecommendationRatingFrequencies()
    {
        AddData();
        var recommendationRatingFrequencies = DoctorFeedbackRepository.Instance.GetRecommendationRatingFrequencies("1");
        Assert.AreEqual(1, recommendationRatingFrequencies[1]);
        Assert.AreEqual(1, recommendationRatingFrequencies[5]);
    }

    [TestMethod]
    public void TestGetDoctorQualityRatingFrequencies()
    {
        AddData();
        var doctorQualityRatingFrequencies = DoctorFeedbackRepository.Instance.GetDoctorQualityRatingFrequencies("1");
        Assert.AreEqual(1, doctorQualityRatingFrequencies[2]);
        Assert.AreEqual(1, doctorQualityRatingFrequencies[5]);
    }

    [TestMethod]
    public void TestGetAverageRatingsByArea()
    {
        AddData();
        var averageRatingsByArea = DoctorFeedbackRepository.Instance.GetAverageRatingsByArea("1");
        Assert.AreEqual(3d, averageRatingsByArea.OverallRating);
        Assert.AreEqual(3d, averageRatingsByArea.RecommendationRating);
        Assert.AreEqual(3.5d, averageRatingsByArea.DoctorQualityRating);
    }
}
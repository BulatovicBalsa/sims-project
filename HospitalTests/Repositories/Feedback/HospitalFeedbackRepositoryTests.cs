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

    [TestMethod]
    public void GetAverageGradesTest()
    {
        HospitalFeedbackRepository.Instance.Add(new HospitalFeedback(2, 1, "", 3, 4, 5));
        HospitalFeedbackRepository.Instance.Add(new HospitalFeedback(3, 1, "", 4, 4, 5));
        HospitalFeedbackRepository.Instance.Add(new HospitalFeedback(2, 1, "", 5, 4, 5));
        HospitalFeedbackRepository.Instance.Add(new HospitalFeedback(3, 1, "", 6, 4, 5));
        var averageGradeByArea = HospitalFeedbackRepository.Instance.GetAverageGrades();

        Assert.AreEqual(2.5, averageGradeByArea.OverallRating);
        Assert.AreEqual(1,averageGradeByArea.RecommendationRating );
        Assert.AreEqual(4.5, averageGradeByArea.ServiceQuality);
        Assert.AreEqual(4, averageGradeByArea.CleanlinessRating);
        Assert.AreEqual(5, averageGradeByArea.PatientSatisfactionRating);
    }
}
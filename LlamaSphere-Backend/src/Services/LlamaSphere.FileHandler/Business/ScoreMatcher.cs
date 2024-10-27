using LlamaSphere.API.DTOs;

namespace LlamaSphere.API.Business;

public class ScoreMatcher
{
    public static decimal CalculateMatchScore(StructuredJob job, StructuredCv cv, Dictionary<string, decimal> weightedKeywords)
    {
        decimal score = 0;

        if (job.PreferredSkills != null && cv.TechnicalSkills != null)
        {
            var requiredSkills = job.PreferredSkills.Select(skill => skill.ToLower()).ToHashSet();
            foreach (var skill in cv.TechnicalSkills)
            {
                if (requiredSkills.Contains(skill.Name.ToLower()))
                {
                    score += skill.Strength;
                }
            }
        }

        foreach (var keyword in weightedKeywords)
        {
            string lowerKeyword = keyword.Key.ToLower();
            decimal weight = keyword.Value;

            if (cv.TechnicalSkills != null && cv.TechnicalSkills.Any(skill => skill.Name.ToLower().Contains(lowerKeyword)))
            {
                score += weight;
            }
            if (cv.Projects.Any(project => project.ToLower().Contains(lowerKeyword)) ||
                cv.Certifications.Any(cert => cert.ToLower().Contains(lowerKeyword)))
            {
                score += weight * 0.6m;
            }
        }

        return score;
    }
}

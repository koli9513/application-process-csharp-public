using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using Codecool.ApplicationProcess.Entities;

namespace Codecool.ApplicationProcess.Data
{
    /// <summary>
    /// Memory storage for application process.
    /// </summary>
    public class XMLRepository : IApplicationRepository
    {
        private readonly XElement _xmlData;

        /// <summary>
        /// Initializes a new instance of the <see cref="XMLRepository"/> class.
        /// </summary>
        public XMLRepository()
        {
            _xmlData = XElement.Load("../../../Resources/Backup.xml");
        }

        /// <inheritdoc/>
        public int AmountOfApplicationAfter(DateTime date)
        {
            return (from applicant in _xmlData.Descendants("Applicants").Descendants("Applicant")
                    where applicant.Element("StartDate").Value != string.Empty && DateTime.Parse(applicant.Element("StartDate").Value) > date
                    select applicant).Count();
        }

        /// <inheritdoc/>
        public IEnumerable<Mentor> GetAllMentorFrom(City city)
        {
            var results = from m in _xmlData.Descendants("Mentors").Descendants("Mentor")
                          where m.Element("City").Value == city.ToString()
                          select new
                          {
                              Id = (int)m.Element("Id"),
                              FirstName = (string)m.Element("FirstName"),
                              LastName = (string)m.Element("LastName"),
                              NickName = (string)m.Element("Nickname"),
                          };
            List<Mentor> mentors = new List<Mentor>();
            foreach (var result in results)
            {
                Mentor mentor = new Mentor
                {
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    Nickname = result.NickName,
                    Id = result.Id,
                };
                mentors.Add(mentor);
            }

            return mentors;
        }

        /// <inheritdoc/>
        public IEnumerable<Mentor> GetAllMentorWhomFavoriteLanguage(string language)
        {
            var results = from m in _xmlData.Descendants("Mentors").Descendants("Mentor")
                          where m.Element("ProgrammingLanguage").Value == language
                          select new
                          {
                              Id = (int)m.Element("Id"),
                              FirstName = (string)m.Element("FirstName"),
                              LastName = (string)m.Element("LastName"),
                              NickName = (string)m.Element("Nickname"),
                          };
            List<Mentor> mentors = new List<Mentor>();
            foreach (var result in results)
            {
                Mentor mentor = new Mentor
                {
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    Nickname = result.NickName,
                    Id = result.Id,
                };
                mentors.Add(mentor);
            }

            return mentors;
        }

        /// <inheritdoc/>
        public IEnumerable<Applicant> GetApplicantsOf(string contactMentorName)
        {
            var name = contactMentorName.Split();

            var results = from a in _xmlData.Descendants("Applications").Descendants("Application")
                          where a.Element("Mentor").Element("FirstName").Value == name[0] && a.Element("Mentor").Element("LastName").Value == name[1]
                          select new
                          {
                              ApplicationCode = (int)a.Element("Applicant").Element("ApplicationCode"),
                              FirstName = (string)a.Element("Applicant").Element("FirstName"),
                              LastName = (string)a.Element("Applicant").Element("LastName"),
                              Id = (int)a.Element("Applicant").Element("Id"),
                          };
            List<Applicant> applicants = new List<Applicant>();
            foreach (var result in results)
            {
                Applicant applicant = new Applicant
                {
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    ApplicationCode = result.ApplicationCode,
                    Id = result.Id,
                };
                applicants.Add(applicant);
            }

            return applicants;
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetAppliedStudentEmailList()
        {
            return from applicant in _xmlData.Descendants("Applicants").Descendants("Applicant")
                   where applicant.Element("Status").Value == ApplicationStatus.Applied.ToString()
                   select applicant.Element("Email").Value;
        }
    }
}
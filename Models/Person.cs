using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ProjetProgAvENSC1A.Services;


namespace ProjetProgAvENSC1A.Models
{
    public class Person : EntryType
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Constants.Gender Gender { get; set; }

        public List<EntryType> Projects => App.DB[DBTable.Project].Entries.Where(entry =>
        {
            Project p = (Project)entry;
            return p.Contributors.ContainsValue((Student)this);
        }).ToList();
    }

    // From: https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to#support-polymorphic-deserialization
    // Defines the way to serialize the Person class with System.Text.Json
    public class PersonConverter : JsonConverter<Person>
    {
        enum TypeDiscriminator
        {
            Student = 1,
            Teacher = 2,
            Extern = 3
        }

        public override bool CanConvert(Type typeToConvert) =>
            typeof(Person).IsAssignableFrom(typeToConvert);

        public override Person Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string propertyName = reader.GetString();
            if (propertyName != "TypeDiscriminator")
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }

            TypeDiscriminator typeDiscriminator = (TypeDiscriminator)reader.GetInt32();
            Person person = typeDiscriminator switch
            {
                TypeDiscriminator.Student => new Student(),
                TypeDiscriminator.Teacher => new Teacher(),
                TypeDiscriminator.Extern => new Extern(),
                _ => throw new JsonException()
            };

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return person;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "Student_ID":
                            string id = reader.GetString();
                            ((Student)person).Student_ID = id;
                            break;
                        case "FirstName":
                            string fname = reader.GetString();
                            person.FirstName = fname;
                            break;
                        case "LastName":
                            string lname = reader.GetString();
                            person.LastName = lname;
                            break;
                        case "Age":
                            int age = reader.GetInt32();
                            person.Age = age;
                            break;
                        case "Gender":
                            int gender = reader.GetInt32();
                            person.Gender = (Constants.Gender)gender;
                            break;
                        case "UUID":
                            string uuid = reader.GetString();
                            person.UUID = uuid;
                            break;
                    }
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Person person, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (person is Student student)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Student);
                writer.WriteString("Student_ID", student.Student_ID);

            }
            else if (person is Teacher teacher)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Teacher);
            }
            else if (person is Extern e)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Extern);
            }

            writer.WriteString("FirstName", person.FirstName);
            writer.WriteString("LastName", person.LastName);
            writer.WriteNumber("Age", person.Age);
            writer.WriteNumber("Gender",(int)person.Gender);
            writer.WriteString("UUID",person.UUID);

            writer.WriteEndObject();
        }
    }
}
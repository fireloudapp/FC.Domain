using System;
using System.Threading.Tasks;
using Bogus;
using FC.Common.DataAccess.Test.Model;
using FC.Common.Domain;
using FC.Extension.SQL.Engine;
using FC.Extension.SQL.Mongo;
using FC.Extension.SQL.Mongo.Helper;
using FC.Extension.SQL.Mongo.Interface;
using MongoDB.Bson;
using Shouldly;
using Xunit;
using Xunit.Abstractions;


namespace FC.Common.DataAccess.Test;

public class MongoDataAccessTest
{
    private readonly ITestOutputHelper _output;
    private readonly INoSQLBaseAccess<PersonMongo> _baseAccess;

    public MongoDataAccessTest(ITestOutputHelper output)
    {
        this._output = output;
        string connection = string.Format(Environment.GetEnvironmentVariable("MongoConnection", EnvironmentVariableTarget.Machine), "TestDB");
        SQLExtension.SQLConfig = new SQLConfig()
        {
            Compiler = SQLCompiler.MongoDB,
            DBType = DBType.NoSQL,
            ConnectionString = connection,
            // ConnectionString =
            //     "mongodb+srv://fc_client_admin:fc.Serverless.mongo@cluster0.acxm4.mongodb.net/TestDB?retryWrites=true&w=majority&connect=replicaSet",
            DataBaseName = "TestDB",
            CollectionName = "TestCollection"
        };
        _baseAccess = new MongoDataAccess<PersonMongo>(sqlConfig: SQLExtension.SQLConfig);
        _output.WriteLine(connection);
    }


    #region  Base Generic CRUD Operation

    [Fact]
    public async Task Create_GenericSave()
    {
        var personFake = new Faker<PersonMongo>()
            .RuleFor(o => o.Name, f => f.Person.FirstName)
            .RuleFor(o => o.Type, f => f.Person.Company.Name)
            .RuleFor(o => o.RegisteredDate, f => f.Date.Past(1))
            .RuleFor(o => o.PersonValue, f => f.Random.Double(1, 10));
        var person = personFake.Generate();
        
        await _baseAccess.ModelCollection.InsertOneAsync(person);
        person.Id.ShouldNotBe(null);
        _output.WriteLine($"Generic ModelCollection based Save {person.ToJson()}");

        User user = new User();

    }

    #endregion
    
    [Fact]
    public void Dummy_Test()
    {
        int index = 100;
        index.ShouldBe(100);
        _output.WriteLine($"Dummy Test");
        
    }
    
    [Fact]
    public void GetByPagingAsync_Test()
    {
        INoSQLBaseAccess<PersonMongo> baseAccess = SQLExtension.GetNoSQLCompiler<PersonMongo>();
        var personList = baseAccess.GetByPagingAsync(per => per.Name, 0, 5).Result;
        foreach (var model in personList)
        {
            _output.WriteLine($"Queried Object : {model.ToJson()}");
        }
    }
}

/*
 *if (IsSave)
            {
                person = person.Save().Result;
                console.Output.WriteLine($"Saved Object : {person.ToJSON<PersonMongo>()}");
            }
            else if (IsUpdate)
            {
                person.Id = GetIdEntry();
                Console.WriteLine($"Unique Id : {person.Id}");
                person = person.Update(per => per.Id == person.Id).Result;
                console.Output.WriteLine($"Object Updated : {person.ToJSON<PersonMongo>()}");
            }
            else if (IsDelete)
            {
                person.Id = GetIdEntry();
                string records = person.Delete(per => per.Id == person.Id, person.Id).Result;
                console.Output.WriteLine($"Deleted. No of Records : {records}");
            }
            else if (IsGetAll)
            {
                var result = person.GetAll().Result;
                foreach (var persons  in result)
                {
                    console.Output.WriteLine(persons.ToJson());
                }
                console.Output.WriteLine("All Records returned.");
            }
            else if (IsGetById)
            {
                person.Id = GetIdEntry();
                PersonMongo per = person.Get(person.Id).Result;
                console.Output.WriteLine($"Received Object : {per.ToJSON()}");
            }
            else if (IsQuery)
            {
                string id = GetIdEntry();
                var personList = person.GetAny
                    (per => per.Id == id).Result;
                foreach (var model in personList)
                {
                    console.Output.WriteLine($"Queried Object : {model.ToJSON()}");
                }
            }
            else if (IsGetCount)
            {
                var personCount = person.GetCount().Result;
                console.Output.WriteLine($"Total Records : {personCount}");
            }
            else if (IsPaged)
            {
                INoSQLBaseAccess<PersonMongo> baseAccess = SQLExtension.GetNoSQLCompiler<PersonMongo>();
                var personList =  baseAccess.GetByPagingAsync(per => per.Name, 0, 5).Result;
                foreach (var model in personList)
                {
                    console.Output.WriteLine($"Queried Object : {model.ToJSON()}");
                }
            }
            else if (IsGeneric)
            {
                INoSQLBaseAccess<PersonMongo> baseAccess = SQLExtension.GetNoSQLCompiler<PersonMongo>();
                //Full object get: Ref: https://docs.mongodb.com/manual/tutorial/query-documents/
                var filter = Builders<BsonDocument>.Filter.Empty; //No filter applied.
                var result = baseAccess.GenericCollection.Find(filter).ToList();
                foreach (var model in result)
                {
                    console.Output.WriteLine($"No Filter Queried Object : {model.ToJSON()}");
                }
                
                //Filter object get
                string id = GetIdEntry();
                filter = Builders<BsonDocument>.Filter.Eq("_id",ObjectId.Parse(id));
                result = baseAccess.GenericCollection.Find(filter).ToList();
                console.Output.WriteLine($"Result : { result.Count}");
                foreach (var model in result)
                {
                    console.Output.WriteLine($"Filtered Query Object : {model.ToJSON()}");
                }
            }
            
 * 
 */
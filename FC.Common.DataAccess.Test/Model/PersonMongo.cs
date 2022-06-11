using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FC.Common.DataAccess.Test.Model;


public class PersonMongo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public Image Image { get; set; }
    public Thumbnail Thumbnail { get; set; }
    public DateTime RegisteredDate { get; set; }
    public double PersonValue { get; set; }
}

public class Image
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class Thumbnail
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

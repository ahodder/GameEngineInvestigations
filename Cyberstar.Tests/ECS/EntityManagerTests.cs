using Cyberstar.ECS;
using Cyberstar.Logging;
using Moq;

namespace Cyberstar.Tests.ECS;

public class EntityManagerTests
{
    [Test]
    public void TestCreateEntity()
    {
        var self = CreateEntityManager();
        
        Assert.That(self.Entities, Is.EqualTo(0));

        var e1 = self.CreateEntity();
        Assert.That(self.Entities, Is.EqualTo(1));
        
        var e2 = self.CreateEntity();
        Assert.That(self.Entities, Is.EqualTo(2));
    }

    [Test]
    public void TestDestroyEntity()
    {
        var self = CreateEntityManager();
        
        var e1 = self.CreateEntity();
        var e2 = self.CreateEntity();
        var e3 = self.CreateEntity();
        
        Assert.That(self.Entities, Is.EqualTo(3));
        self.DestroyEntity(e1);
        Assert.That(self.Entities, Is.EqualTo(2));
        Assert.That(self.DestroyedEntities, Is.EqualTo(1));
        Assert.That(self.DoesEntityExist(e1), Is.False);
        Assert.That(self.DoesEntityExist(e2), Is.True);
        Assert.That(self.DoesEntityExist(e3), Is.True);

        self.DestroyEntity(e3);
        Assert.That(self.Entities, Is.EqualTo(1));
        Assert.That(self.DestroyedEntities, Is.EqualTo(2));
        Assert.That(self.DoesEntityExist(e1), Is.False);
        Assert.That(self.DoesEntityExist(e2), Is.True);
        Assert.That(self.DoesEntityExist(e3), Is.False);
        
        self.DestroyEntity(e2);
        Assert.That(self.Entities, Is.EqualTo(0));
        Assert.That(self.DestroyedEntities, Is.EqualTo(3));
        Assert.That(self.DoesEntityExist(e1), Is.False);
        Assert.That(self.DoesEntityExist(e2), Is.False);
        Assert.That(self.DoesEntityExist(e3), Is.False);
    }
    
    [Test]
    public void TestSetParentFor()
    {
        var self = CreateEntityManager();

        var parent = self.CreateEntity();
        
        var cnt = 8;
        var entities = new List<Entity>();
        for (var i = 0; i < cnt; i++)
            entities.Add(self.CreateEntity());
        
        
        Assert.That(self.GetChildrenCount(parent), Is.EqualTo(0));

        for (var i = 0; i < cnt; i++)
        {
            self.SetParentFor(parent, entities[i]);
            Assert.That(self.GetChildrenCount(parent), Is.EqualTo(i + 1));
        }
    }
    
    [Test]
    public void TestRemoveChildFor()
    {
        var self = CreateEntityManager();

        var e = self.CreateEntity();
        var c1 = self.CreateEntity();
        var c2 = self.CreateEntity();
        var c3 = self.CreateEntity();

        Assert.True(self.SetParentFor(e, c1));
        Assert.True(self.SetParentFor(e, c2));
        Assert.True(self.SetParentFor(e, c3));
        
        Assert.That(self.GetChildrenCount(e), Is.EqualTo(3));
        Assert.That(self.IsChildOf(e, c1));
        Assert.That(self.IsChildOf(e, c2));
        Assert.That(self.IsChildOf(e, c3));

        Assert.True(self.RemoveChildFor(e, c3));
        Assert.That(self.GetChildrenCount(e), Is.EqualTo(2));
        Assert.That(self.IsChildOf(e, c1));
        Assert.That(self.IsChildOf(e, c2));
        
        Assert.True(self.RemoveChildFor(e, c1));
        Assert.That(self.GetChildrenCount(e), Is.EqualTo(1));
        Assert.That(self.IsChildOf(e, c2));
        
        Assert.True(self.RemoveChildFor(e, c2));
        Assert.That(self.GetChildrenCount(e), Is.EqualTo(0));
    }

    private EntityManager CreateEntityManager()
    {
        var logger = new Mock<ILogger>();
        var self = new EntityManager(logger.Object, 8);
        return self;
    }
}
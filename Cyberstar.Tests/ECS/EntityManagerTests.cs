using Cyberstar.ECS;
using Cyberstar.Engine.Logging;
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

    [Test]
    public void TestGetComponents()
    {
        var self = CreateEntityManager();

        var e1 = self.CreateEntity();

        Assert.False(self.HasComponentFor<FloatComponent>(e1));
        Assert.False(self.HasComponentFor<IntComponent>(e1));
        
        self.SetComponentFor(e1, new FloatComponent(3.14f));
        self.SetComponentFor(e1, new IntComponent(1337));

        Assert.True(self.TryGetComponentFor<FloatComponent>(e1, out var fc));
        Assert.True(self.TryGetComponentFor<IntComponent>(e1, out var ic));

        Assert.That(fc.Float, Is.EqualTo(3.14f));
        Assert.That(ic.Int, Is.EqualTo(1337));
    }

    // [Test]
    // public void TestManipulatingComponentsRetainsData()
    // {
    //     var em = CreateEntityManager();
    //     var e1 = CreateEntityWith(em, 1.5f, 15);
    //     var e2 = CreateEntityWith(em, 3f, 30);
    //     var e3 = CreateEntityWith(em, 4.5f, 45);
    //     var e4 = CreateEntityWith(em, 6f, 60);
    //     
    //     Assert.True(em.DoesEntityExist(e1));
    //     Assert.True(em.DoesEntityExist(e2));
    //     Assert.True(em.DoesEntityExist(e4));
    //
    //     Assert.True(em.DoesEntityExist(e1));
    //     Assert.True(em.DoesEntityExist(e2));
    //     Assert.False(em.DoesEntityExist(e3));
    //     Assert.True(em.DoesEntityExist(e4));
    //     
    //     Assert.That(em.Get)
    // }

    private EntityManager CreateEntityManager()
    {
        var logger = new Mock<ILogger>();
        var self = new EntityManager(logger.Object, 8);
        return self;
    }

    private Entity CreateEntityWith(EntityManager em, float f, int i)
    {
        var ret = em.CreateEntity();
        em.SetComponentsFor(ret, new FloatComponent(f), new IntComponent(i));
        return ret;
    }
}
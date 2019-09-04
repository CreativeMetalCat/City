using Box2DX.Dynamics;
using System.Linq;

namespace City.Engine.Physics
{
    public class ContactListener : Box2DX.Dynamics.ContactListener
    {
        public override void Add(ContactPoint point)
        {
            base.Add(point);


            if (point.Shape1.UserData != point.Shape2.UserData)
            {
                if ((point.Shape1.UserData is Actor)) { (point.Shape1.UserData as Actor).Components.OfType<Components.Physics.PhysicsBodyComponent>().Single< Components.Physics.PhysicsBodyComponent>().OnBeginContact((point.Shape2.UserData as Actor), point.Shape1, point.Shape2); }
                else { System.Diagnostics.Debug.WriteLine("Contact listener warning: This contact listener requires for user data to be owner of the component to properly operate. In ContactListener.Add"); }
                if ((point.Shape2.UserData is Actor)) { (point.Shape2.UserData as Actor).Components.OfType<Components.Physics.PhysicsBodyComponent>().Single<Components.Physics.PhysicsBodyComponent>().OnBeginContact((point.Shape1.UserData as Actor), point.Shape2, point.Shape1); }
                else { System.Diagnostics.Debug.WriteLine("Contact listener warning: This contact listener requires for user data to be owner of the component to properly operate. In ContactListener.Add"); }
            }
            else
            {
                //collision within istelf
            }
        }

        public override void Remove(ContactPoint point)
        {
            base.Remove(point);

            if (point.Shape1.UserData != point.Shape2.UserData)
            {
                if ((point.Shape1.UserData is Actor)) { (point.Shape1.UserData as Actor).Components.OfType<Components.Physics.PhysicsBodyComponent>().Single<Components.Physics.PhysicsBodyComponent>().OnEndContact((point.Shape2.UserData as Actor), point.Shape1, point.Shape2); }
                else { System.Diagnostics.Debug.WriteLine("Contact listener warning: This contact listener requires for user data to be owner of the component to properly operate. In ContactListener.Add"); }
                if ((point.Shape2.UserData is Actor)) { (point.Shape2.UserData as Actor).Components.OfType<Components.Physics.PhysicsBodyComponent>().Single<Components.Physics.PhysicsBodyComponent>().OnEndContact((point.Shape1.UserData as Actor), point.Shape2, point.Shape1); }
                else { System.Diagnostics.Debug.WriteLine("Contact listener warning: This contact listener requires for user data to be owner of the component to properly operate. In ContactListener.Add"); }
            }
            else
            {
                //collision within istelf
            }
        }
        /// <summary>
        /// How it supposed be (in proper c++ version)
        /// </summary>
        /// <param name="contact"></param>
        void OnBeginContact(Box2DX.Dynamics.Contact contact)
        {
            if ((contact.GetShape1().UserData is Actor) && (contact.GetShape2().UserData is Actor))
            {
                (contact.GetShape1().UserData as Actor).OnBeginContact((contact.GetShape2().UserData as Actor), contact.GetShape1(), contact.GetShape2());
                (contact.GetShape2().UserData as Actor).OnBeginContact((contact.GetShape1().UserData as Actor), contact.GetShape2(), contact.GetShape1());

            }
            else
            {
                throw new Exceptions.PhysicsBodyDataIsBroken("This contact listener requires for user data to be owner of the component to properly operate");
            }
        }


        void OnEndContact(Box2DX.Dynamics.Contact contact)
        {
            if ((contact.GetShape1().UserData is Actor) && (contact.GetShape2().UserData is Actor))
            {
                (contact.GetShape1().UserData as Actor).OnEndContact((contact.GetShape2().UserData as Actor), contact.GetShape1(), contact.GetShape2());
                (contact.GetShape2().UserData as Actor).OnEndContact((contact.GetShape1().UserData as Actor), contact.GetShape2(), contact.GetShape1());
            }
            else
            {
                throw new Exceptions.PhysicsBodyDataIsBroken("This contact listener requires for user data to be owner of the component to properly operate");
            }
        }
    }
}

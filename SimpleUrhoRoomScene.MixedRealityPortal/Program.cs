using System;
using Windows.ApplicationModel.Core;
using Urho;
using Urho.HoloLens;
using Urho.Resources;

namespace SimpleUrhoRoomScene.MixedReality
{
	internal class Program
	{
		[MTAThread]
		static void Main()
		{
			var appViewSource = new UrhoAppViewSource<HelloWorldApplication>(new ApplicationOptions("Data"));
			appViewSource.UrhoAppViewCreated += OnViewCreated;
			CoreApplication.Run(appViewSource);
		}

		static void OnViewCreated(UrhoAppView view) { }
	}

	public class HelloWorldApplication : HoloApplication
	{
		Node roomNode;

		public HelloWorldApplication(ApplicationOptions opts) : base(opts) { }

		protected override async void Start()
		{
			EnableGestureManipulation = true;
			EnableGestureTapped = true;

			base.Start();

			Zone.FogColor = Color.White;
			Zone.SetBoundingBox(new BoundingBox(-1000, 1000));
			Zone.AmbientColor = new Color(0.6f, 0.6f, 0.6f);

			var skyboxNode = Scene.CreateChild();
			var skybox = skyboxNode.CreateComponent<Skybox>();
			skybox.Model = CoreAssets.Models.Box;
			skybox.SetMaterial(ResourceCache.GetMaterial("Materials/Skybox.xml"));

			roomNode = Scene.InstantiateXml(ResourceCache.GetFile("Scenes/RoomPrefab.xml", true), new Vector3(-0.5f, -1.5f, 1.5f), Quaternion.Identity, CreateMode.Local);
			roomNode.Rotation = new Quaternion(0, 90, 0);
			roomNode.SetScale(0.08f);
		}

		Vector3 earthPosBeforeManipulations;
		public override void OnGestureManipulationStarted() => earthPosBeforeManipulations = roomNode.Position;
		public override void OnGestureManipulationUpdated(Vector3 relativeHandPosition) =>
			roomNode.Position = relativeHandPosition + earthPosBeforeManipulations;
	}
}
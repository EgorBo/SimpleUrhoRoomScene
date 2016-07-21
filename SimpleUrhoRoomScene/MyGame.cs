using Urho;

namespace SimpleUrhoRoomScene
{
	public class MyGame : Application
	{
		Scene scene;
		Node cameraNode;
		Camera camera;
		bool isMobile;
		float yaw = -90.0f;
		float pitch;

		public MyGame() : base(new ApplicationOptions(new[] {"Data"})) { }

		protected override void Start()
		{
			scene = new Scene();
			scene.LoadXml(ResourceCache.GetFile(Assets.Scenes.Room, true));

			cameraNode = scene.CreateChild();
			camera = cameraNode.CreateComponent<Camera>();
			cameraNode.Position = new Vector3(15.0f, 10.0f, 0.0f);
			camera.Fov = 70.0f;

			Node weaponNode = cameraNode.CreateChild();
			weaponNode.SetScale(0.1f);
			weaponNode.Position = new Vector3(0.7f, -1.3f, 1.2f);
			StaticModel weaponObject = weaponNode.CreateComponent<StaticModel>();
			weaponObject.Model = ResourceCache.GetModel(Assets.Models.Weapon);
			weaponObject.ApplyMaterialList("");

			Viewport viewport = new Viewport(scene, camera, null);
			viewport.RenderPath.Append(CoreAssets.PostProcess.FXAA2);
			Renderer.SetViewport(0, viewport);

			// Subscribe to Esc key:
			Input.SubscribeToKeyDown(args => { if (args.Key == Key.Esc) Exit(); });

			isMobile = Platform == Platforms.Android || Platform == Platforms.iOS;
			//if (isMobile)
			{
				// Add joystick
				var layout = ResourceCache.GetXmlFile(Assets.UI.ScreenJoystick_Samples);
				var screenJoystickIndex = Input.AddScreenJoystick(layout, ResourceCache.GetXmlFile(Assets.UI.DefaultStyle));
				Input.SetScreenJoystickVisible(screenJoystickIndex, true);
			}
		}

		protected override void OnUpdate(float timeStep)
		{
			const float moveSpeed = 20.0f;
			if (isMobile)
				MoveCameraByTouches(timeStep);
			else
				MoveCameraByMouse(timeStep);

			if (Input.GetKeyDown(Key.W)) cameraNode.Translate(new Vector3(0.0f, 0.0f, 1.0f) * moveSpeed * timeStep);
			if (Input.GetKeyDown(Key.S)) cameraNode.Translate(new Vector3(0.0f, 0.0f, -1.0f) * moveSpeed * timeStep);
			if (Input.GetKeyDown(Key.A)) cameraNode.Translate(new Vector3(-1.0f, 0.0f, 0.0f) * moveSpeed * timeStep);
			if (Input.GetKeyDown(Key.D)) cameraNode.Translate(new Vector3(1.0f, 0.0f, 0.0f) * moveSpeed * timeStep);
		}

		void MoveCameraByTouches(float timeStep)
		{
			const float touchSensitivity = 2;
			var input = Input;
			for (uint i = 0, num = input.NumTouches; i < num; ++i)
			{
				TouchState state = input.GetTouch(i);
				if (state.Delta.X != 0 || state.Delta.Y != 0)
				{
					yaw += touchSensitivity * camera.Fov / Graphics.Height * state.Delta.X;
					pitch += touchSensitivity * camera.Fov / Graphics.Height * state.Delta.Y;
					cameraNode.Rotation = new Quaternion(pitch, yaw, 0);
				}
			}
		}

		void MoveCameraByMouse(float timeStep)
		{
			var mouseMove = Input.MouseMove;
			const float mouseSensitivity = 0.1f;
			yaw += mouseSensitivity * mouseMove.X;
			pitch += mouseSensitivity * mouseMove.Y;
			pitch = MathHelper.Clamp(pitch, -90.0f, 90.0f);
			cameraNode.Rotation = new Quaternion(pitch, yaw, 0.0f);
		}
	}
}
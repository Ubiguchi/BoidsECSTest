namespace BoidsECSTest.NezEcs.Components
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Nez;
	using Nez.Sprites;
	using Nez.Textures;

	public class DrawSystemRenderer : SpriteRenderer
	{
		public DrawSystemRenderer(Texture2D texture)
			: this(new Sprite(texture))
		{
		}

		public DrawSystemRenderer(Sprite sprite)
			: base(sprite)
		{
			Material = new Material { SamplerState = SamplerState.LinearWrap };
		}

		public override void Render(Batcher batcher, Camera camera)
		{
			if (_sprite == null)
			{
				return;
			}

			var drawInfo = Entity.GetComponent<DrawInfo>();
			batcher.Draw(_sprite, drawInfo.Position, null, drawInfo.Color, drawInfo.Rotation, new Vector2(.5f), drawInfo.Size, SpriteEffects.None, 0f);
		}
	}
}
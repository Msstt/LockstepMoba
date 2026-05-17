using Combat.Fog;
using UnityEngine;
using UnityEngine.UI;

public class FogTestTexture : MonoBehaviour {
    private Image image;

    public void Start() {
        image = GetComponent<Image>();
        Texture2D texture = GameMgr.Instance.GetSystem<IFogSystem>().FogTexture;
        image.sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );
    }
}

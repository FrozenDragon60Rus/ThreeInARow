using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Cells;


// Merge multiple Sprites as one sprite
namespace Assets.Script
{
    public static class MergeSprite
    {
        public static Sprite Overlay(Sprite[] sprites)
        {
            // 스프라이트 중 가장 큰 크기의 것으로 지정되야함
            int width = 0;
            int height = 0;


            foreach (var sprite in sprites)
            {
                width = Mathf.Max(width, sprite.texture.width);
                height = Mathf.Max(height, sprite.texture.height);
            }

            Texture2D mergedTexture = new Texture2D(width, height);
            // mergedTexture.alphaIsTransparency = true;

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    mergedTexture.SetPixel(i, j, Color.clear);

            // overdrawn by last one
            foreach (var sprite in sprites)
            {
                for (int y = 0; y < sprite.texture.height; y++)
                    for (int x = 0; x < sprite.texture.width; x++)
                    {
                        Color extractedPixel = sprite.texture.GetPixel(x, y);

                        Color replacedColor = mergedTexture.GetPixel(x, y);

                        replacedColor = (1.0f - extractedPixel.a) * replacedColor + extractedPixel.a * extractedPixel;

                        mergedTexture.SetPixel(x, y, replacedColor);
                    }

                mergedTexture.Apply();
            }


            Sprite mergedSprite = Sprite.Create(mergedTexture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));

            return mergedSprite;
        }

        public static Sprite Join(Sprite sprite, Sprite currentSprite, List<Cell> cell)
        {
            int width = 50,
                height = 50;

            Texture2D mergedTexture = currentSprite.texture;
            Debug.Log(sprite.texture.width + " " + sprite.texture.height);

            for (int i = 0; i < mergedTexture.width; i++)
                for (int j = 0; j < mergedTexture.height; j++)
                    mergedTexture.SetPixel(i, j, Color.clear);

            foreach (Cell _cell in cell)
            {
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        Color extractedPixel = sprite.texture.GetPixel(x, y);
                        Color replacedColor = mergedTexture.GetPixel(_cell.col * width + x, _cell.row * height + y);

                        replacedColor = (1.0f - extractedPixel.a) * replacedColor + extractedPixel.a * extractedPixel;

                        mergedTexture.SetPixel(x, y, replacedColor);
                    }
            }
            Debug.Log(currentSprite.texture.width + " " + currentSprite.texture.height);
            mergedTexture.Apply();
            Sprite mergedSprite = Sprite.Create(mergedTexture, currentSprite.textureRect, currentSprite.pivot, currentSprite.pixelsPerUnit);

            return mergedSprite;
        }
    }
}
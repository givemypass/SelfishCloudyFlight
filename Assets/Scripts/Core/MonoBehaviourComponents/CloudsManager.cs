using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.MonoBehaviourComponents
{
    public class CloudsManager : MonoBehaviour
    {
        private struct Cloud
        {
            public SpriteRenderer SpriteRenderer;
            public float Speed;
        }
        [SerializeField] private Sprite[] cloudSprites;
        [SerializeField] private float zOffset;
        [SerializeField] private Vector2 sizeRange;
        [SerializeField] private Vector2 speedRange;
        [SerializeField] private Vector2 alphaRange;
        [SerializeField] private GameObject cloudPrefab;
        
        [SerializeField] private int cloudsCount;
        
        private readonly List<Cloud> clouds = new(); 

        void Start()
        {
            SpawnClouds();
        }

        private void Update()
        {
            foreach (var cloud in clouds)
            {
                var position = cloud.SpriteRenderer.transform.position;
                // position += Vector3.right * (cloud.Speed * Time.deltaTime);
                // cloud.SpriteRenderer.transform.position = position;

                var camera = Camera.main;
                var width = cloud.SpriteRenderer.bounds.size.x;
                
                if (cloud.Speed > 0 && position.x - width / 2f > camera.orthographicSize * camera.aspect 
                    || cloud.Speed < 0 && position.x + width / 2f < -camera.orthographicSize * camera.aspect)
                {
                    Respawn(cloud, camera);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            //draw wire cube for each cloud
            foreach (var cloud in clouds)
            {
                var position = cloud.SpriteRenderer.transform.position;
                var size = cloud.SpriteRenderer.bounds.size;
                Gizmos.DrawWireCube(position, new Vector3(size.x, size.y, 1));
            }
        }

        private void Respawn(Cloud cloud, Camera camera)
        {
            var position = cloud.SpriteRenderer.transform.position;
            var sprite = GetRandomUniqSprite();
            cloud.SpriteRenderer.sprite = sprite;
            var width = cloud.SpriteRenderer.bounds.size.x;
            var xPos =
                cloud.Speed > 0 ?
                -camera.orthographicSize * camera.aspect - width / 2f
                : camera.orthographicSize * camera.aspect + width / 2f;
            
            position.x = xPos;
            cloud.SpriteRenderer.transform.position = position;
        }

        void SpawnClouds()
        {
            for (int i = 0; i < cloudsCount; i++)
            {
                var sprite = GetRandomUniqSprite();
            
                var cloud = Instantiate(cloudPrefab, transform);
                var spriteRenderer = cloud.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;
                spriteRenderer.color = new Color(1,1,1, Random.Range(alphaRange.x, alphaRange.y));
                
                clouds.Add(new Cloud
                {
                    SpriteRenderer = spriteRenderer,
                    Speed = Random.Range(speedRange.x, speedRange.y) * (i % 2 == 0 ? -1 : 1)
                });

                var screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
                var screenHeight = Camera.main.orthographicSize;

                var yStep = screenHeight * 2f / cloudsCount;
                
                var randomX = i % 2 == 0 ? Random.Range(0, screenWidth) : Random.Range(-screenWidth, 0);
                var randomY = yStep * i - screenHeight + yStep / 2f;

                cloud.transform.position = new Vector3(randomX, randomY, zOffset); // Задаем позицию
                var size = Random.Range(sizeRange.x, sizeRange.y);
                cloud.transform.localScale = new Vector3(size, size, 1);
            }
        }

        private Sprite GetRandomUniqSprite()
        {
            var index = Random.Range(0, cloudSprites.Length);
            for (int i = 0; i < cloudSprites.Length; i++)
            {
                if (CheckUniqIndex(index))
                {
                    break;
                }

                index++;
                if (index >= cloudSprites.Length)
                    index = 0;
            }
            
            return cloudSprites[index];
        }

        private bool CheckUniqIndex(int index)
        {
            foreach (var cloud in clouds)
            {
                if (cloud.SpriteRenderer.sprite != cloudSprites[index])
                    continue;

                return false;
            }

            return true;
        }
    }
}
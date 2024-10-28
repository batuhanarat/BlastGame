using System;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int _pos;
    public ItemBase? _currentItem;
    private Func<Vector2Int,List<Cell>> _getMatchGroup;
    private Func<bool> isMoveAvailable;
    public Action onExplode;
    protected ParticleSystem particleSystem;

    [SerializeField] private GameSettings settings;


    public void Awake()
    {
        settings = Resources.Load<GameSettings>("ScriptableObjects/SettingSO");
    }

    public void Initialize(Vector2Int position, Func<Vector2Int, List<Cell>> GetMatchGroup, Func<bool> IsMoveAvailable, Action OnExplode) {
        _pos = position;
        _getMatchGroup = GetMatchGroup;
        isMoveAvailable = IsMoveAvailable;
        onExplode = OnExplode;
    }
    public bool IsItemPresent() {
        return _currentItem != null;
    }
    public ItemBase GetItem() {
        return _currentItem;
    }
    public void SetItem(ItemBase newItem, bool placeToCell = true) {
        if(newItem != null) {
            _currentItem = newItem;
            if(placeToCell) {
                newItem.PlaceInCell(this,_pos);
            }
        } else {
            _currentItem = null;
        }

    }
    public void RemoveItemFromCell() {
        var item = transform.GetChild(0);
    }
    public Vector2Int GetIndex() {
        return _pos;
    }

    public void  PlayParticleEffect(List<Sprite> sprites) {
        particleSystem = GetComponent<ParticleSystem>();
        ParticleSystem.TextureSheetAnimationModule tsam = particleSystem.textureSheetAnimation;
        tsam.mode = ParticleSystemAnimationMode.Sprites;
        for (int i = tsam.spriteCount - 1; i >= 0; i--) {
            tsam.RemoveSprite(i);
        }
        for (int i = 0; i < sprites.Count; i++) {
            tsam.AddSprite(sprites[i]);
        }
        particleSystem.Play();
    }


    public void PlayParticleEffect(Sprite sprite) {
        particleSystem = GetComponent<ParticleSystem>();
        ParticleSystem.TextureSheetAnimationModule tsam = particleSystem.textureSheetAnimation;
        tsam.mode = ParticleSystemAnimationMode.Sprites;

        for (int i = tsam.spriteCount - 1; i >= 0; i--) {
            tsam.RemoveSprite(i);
        }
        tsam.AddSprite(sprite);

        particleSystem.Play();

    }
    public void OnMouseDown()
    {

        if(settings.inDebugMode) {
            var ispresent = IsItemPresent();
            if(ispresent) {
                var sr = _currentItem.GetComponent<SpriteRenderer>();

                Debug.Log("world position" +_currentItem.transform.position + "grid position: " + _currentItem._position+ " type: " + _currentItem.Type +" color: " +_currentItem.Color + "spirte on" +sr.enabled + " move: "+isMoveAvailable());
            }
            return;
        }
        if(!isMoveAvailable() || !IsItemPresent()) {return;}

        if(_currentItem is IInteractable) {
            List<Cell> matchGroup = _getMatchGroup(_pos);
            CellVisitTracker.Reset();
            bool isExploded = _currentItem.TryExplode(_currentItem.Type,matchGroup);
            if(isExploded){
                onExplode();
            }
        }
        else {
            _currentItem.PlayInvalidMoveAnimation(_currentItem.transform, 0.3f);
        }
        CellVisitTracker.Reset();


    }

}
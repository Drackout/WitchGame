using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RequestButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] RequestDifficulty requestDifficulty;
    [SerializeField] Image[] enemySlots;
    [SerializeField] Image[] elementSlots;
    [SerializeField] Sprite fireElement;
    [SerializeField] Sprite waterElement;
    [SerializeField] Sprite grassElement;
    [SerializeField] private RequestInfoBox infoBox;
    private Animator Animator;

    void Start()
    {
        Animator = gameObject.GetComponentInChildren<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BattleSettings settings = BattleSettings.Instance;
        settings.ChooseRequest(requestDifficulty);

        RequestData request = settings.CurrentRequest;
        IList<EnemyCreature> enemies = settings.GetAllEnemiesRequests(request);
        IEnumerable<Element> types = enemies.Select((EnemyCreature e) => e.element);
        infoBox.Show(request.location, (int)requestDifficulty + 1, types,
            request.story, request.thumbnail);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.Hide();
    }
}

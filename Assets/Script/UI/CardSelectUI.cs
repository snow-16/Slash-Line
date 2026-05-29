using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectUI : ChildSystem
{
    [SerializeField]
    private Sprite[] card_images = new Sprite[4];

    private CardSlot[] cards = new CardSlot[3];

    protected override void Start()
    {
        base.Start();
        for(int i = 0; i < 3; i++)
        {
            cards[i] = new CardSlot(transform.GetChild(i).GetComponent<SelectableCard>());
        }
    }

    public void DrawCard()
    {
        List<CrystalAndBlades.CAndBDatas>[] drawables = Mother.AllDatas;

        for(int i = 0; i < 3; i++)
        {
            CrystalAndBlades.CAndBRarelity rarelity;
            do
            {
            rarelity = Mother.RunDrawRandmizer();
            }
            while(drawables[(int)rarelity].Count == 0);
            List<CrystalAndBlades.CAndBDatas> drawedRarelity = drawables[(int)rarelity];

            int drawIndex = Random.Range(0, drawedRarelity.Count);
            CrystalAndBlades.CAndBDatas card = drawedRarelity[drawIndex];
            cards[i].DrawCard(card, card_images[(int)rarelity]);
            cards[i].Card.gameObject.SetActive(true);

            drawedRarelity.RemoveAt(drawIndex);
            drawables[(int)rarelity] = drawedRarelity;
        }

        Mother.AllDatas = drawables;
    }

    public void HideCard()
    {
        foreach(CardSlot card in cards)
        {
            card.Card.gameObject.SetActive(false);
        }
    }

    public struct CardSlot
    {
        private SelectableCard card;
        private Image card_image;
        private Image blade_image;
        private Image[] crystal_images;
        private TMPro.TextMeshProUGUI weight_view;
        private TMPro.TextMeshProUGUI damage_view;
        private TMPro.TextMeshProUGUI detail_text;

        public CardSlot(SelectableCard drawed_card)
        {
            card = drawed_card;
            card_image = card.GetComponent<Image>();
            blade_image = card.transform.GetChild(0).GetComponent<Image>();
            crystal_images = new Image[4];
            Transform crystal_base = card.transform.GetChild(1);
            for(int i = 0; i < 4; i++)
            {
                crystal_images[i] = crystal_base.GetChild(i).GetComponent<Image>();
            }

            Transform info = card.transform.GetChild(2);
            weight_view = info.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
            damage_view = info.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
            detail_text = info.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
        }

        public void DrawCard(CrystalAndBlades.CAndBDatas data, Sprite cardSprite)
        {
            card_image.sprite = cardSprite;
            blade_image.color = data.crystal_color;
            foreach(var crystal_image in crystal_images)
            {
                crystal_image.color = data.crystal_color;
            }
            weight_view.text = data.weight.ToString();
            damage_view.text = data.damage.ToString();

            string[] datail_base = data.detail.ToString().Split('|');
            string fixed_text = "";
            foreach(string parts in datail_base)
            {
                fixed_text += parts + "\n";
            }
            detail_text.text = fixed_text;
            card.Type = data.type;
        }

        public SelectableCard Card
        {
            get{return card;}
        }
    }
}

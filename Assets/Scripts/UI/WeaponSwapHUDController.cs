using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSwapHUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject weaponInfoPrefab;
    //[SerializeField] private RectTransform equippedTransform, previousTransform, nextTransform;
    [SerializeField] private List<RectTransform> weaponSlots;

    private List<WeaponHUDObject> hudObjects = new List<WeaponHUDObject>();
    private CanvasGroup cg;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        foreach (WeaponHUDObject hudObj in hudObjects)
            hudObj.Tick();
    }

    public void RepopulateHUD(List<WeaponController> weapons, int activeWeaponIndex)
    {
        if (hudObjects.Count < weapons.Count)
            RepopulateHudObjects();

        foreach (WeaponHUDObject hudObj in hudObjects)
            hudObj.Populate(null);

        StopAllCoroutines();
        cg.alpha = 1f;

        for (int i = 0; i < weapons.Count; i++)
        {
            if (i >= weaponSlots.Count)
                continue;

            hudObjects[i].Populate(weapons[i]);
            hudObjects[i].SetParent(weaponSlots[i]);
            hudObjects[i].SetAlpha(0.4f);
            hudObjects[i].SetOffset(Vector3.zero);

            if (i == activeWeaponIndex)
            {
                hudObjects[i].SetAlpha(1f);
                hudObjects[i].SetOffset(Vector3.left * 30f);
            }
        }
    }

    public void RepopulateHudObjects()
    {
        for (int i = hudObjects.Count - 1; i >= 0; i--)
            Destroy(hudObjects[i].instance);

        hudObjects.Clear();

        for (int i = 0; i < ServiceLocator.instance.GetService<PlayerWeaponSystem>().weaponSlotCount; i++)
        {
            hudObjects.Add(new WeaponHUDObject(weaponInfoPrefab, transform, null));
        }
    }

    public class WeaponHUDObject
    {
        public TextMeshProUGUI nameTxt, modifierTxt, ammoTxt;
        public Image effectIcon;
        public CanvasGroup cg;
        public GameObject instance;
        public RectTransform target;

        private float destAlpha;
        private Vector3 offset;

        public WeaponHUDObject(GameObject prefab, Transform parent, WeaponController weapon)
        {
            instance = Instantiate(prefab, parent);

            nameTxt = modifierTxt = ammoTxt = null;
            effectIcon = null;

            target = null;
            destAlpha = 0f;

            cg = instance.GetComponent<CanvasGroup>();
            cg.alpha = 0f;

            nameTxt = GetChildComponentOfName<TextMeshProUGUI>("Weapon Name");
            modifierTxt = GetChildComponentOfName<TextMeshProUGUI>("Modifiers List");
            effectIcon = GetChildComponentOfName<Image>("Effect Icon");

            // This actually represents the additive delay instead
            ammoTxt = GetChildComponentOfName<TextMeshProUGUI>("Delay Text");

            Populate(weapon);
        }

        public void Populate(WeaponController weapon)
        {
            if (weapon == null)
            {
                destAlpha = 0f;
            }
            else
            {
                int[] dna = weapon.dna;

                nameTxt.text = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetWeaponObject(dna).displayName;
                effectIcon.sprite = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetEffectObject(dna).icon;

                // This actually represents the additive delay instead
                ammoTxt.text = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetModifierAdditiveDelay(dna).ToString();

                string modifierDescription = "";
                foreach (ProjectileModifier m in ServiceLocator.instance.GetService<WeaponComponentProvider>().GetProjectileModifiers(dna))
                {
                    modifierDescription += m.ToString() + "\n";
                }

                modifierTxt.text = modifierDescription;
            }
        }

        public void SetAlpha(float alpha)
        {
            destAlpha = alpha;
        }

        public void SetParent(RectTransform rt)
        {
            //target = rt;
            instance.transform.SetParent(rt);
        }

        public void SetOffset(Vector3 offset)
        {
            this.offset = offset;
        }

        public void Tick()
        {
            // if (target)
            //     instance.transform.localPosition = Vector3.Lerp(instance.transform.localPosition, target.localPosition, Time.deltaTime * 5f);

            cg.alpha = Mathf.Lerp(cg.alpha, destAlpha, Time.deltaTime * 6f);
            instance.transform.localPosition = Vector3.Lerp(instance.transform.localPosition, Vector3.zero + offset, Time.deltaTime * 3f);
        }

        private T GetChildComponentOfName<T>(string name)
        {
            for (int i = 0; i < instance.transform.childCount; i++)
            {
                if (instance.transform.GetChild(i).name == name)
                    return instance.transform.GetChild(i).GetComponent<T>();
            }

            return default(T);
        }

    }
}

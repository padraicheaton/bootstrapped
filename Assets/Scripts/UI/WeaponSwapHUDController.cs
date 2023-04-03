using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSwapHUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject weaponInfoPrefab;
    [SerializeField] private RectTransform equippedTransform, previousTransform, nextTransform;

    private List<WeaponHUDObject> hudObjects = new List<WeaponHUDObject>();
    private CanvasGroup cg;

    private void Start()
    {
        for (int i = 0; i < ServiceLocator.instance.GetService<PlayerWeaponSystem>().weaponSlotCount; i++)
        {
            hudObjects.Add(new WeaponHUDObject(weaponInfoPrefab, transform, null));
        }

        cg = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        foreach (WeaponHUDObject hudObj in hudObjects)
            hudObj.Tick();
    }

    public void RepopulateHUD(List<WeaponController> weapons, int activeWeaponIndex)
    {
        foreach (WeaponHUDObject hudObj in hudObjects)
            hudObj.Populate(null);

        StopAllCoroutines();
        StartCoroutine(ShowForDuration(3f));

        int prevIndex = activeWeaponIndex - 1;

        if (prevIndex < 0)
            prevIndex = weapons.Count - 1;

        int nextIndex = activeWeaponIndex + 1;

        if (nextIndex >= weapons.Count)
            nextIndex = 0;

        for (int i = 0; i < weapons.Count; i++)
        {
            hudObjects[i].Populate(weapons[i]);

            RectTransform parent = null;

            if (i == activeWeaponIndex)
            {
                hudObjects[i].SetAlpha(1f);
                parent = equippedTransform;
            }
            else if (i == prevIndex)
            {
                hudObjects[i].SetAlpha(0.25f);
                parent = previousTransform;
            }
            else if (i == nextIndex)
            {
                hudObjects[i].SetAlpha(0.25f);
                parent = nextTransform;
            }
            else
                hudObjects[i].SetAlpha(0f);

            if (parent)
                hudObjects[i].SetTargetPos(parent);
        }
    }

    private IEnumerator ShowForDuration(float duration)
    {
        cg.alpha = 1f;

        yield return new WaitForSeconds(duration);

        while (cg.alpha > 0)
        {
            cg.alpha -= Time.deltaTime;

            yield return null;
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

        public void SetTargetPos(RectTransform rt)
        {
            //target = rt;
            instance.transform.SetParent(rt);
        }

        public void Tick()
        {
            // if (target)
            //     instance.transform.localPosition = Vector3.Lerp(instance.transform.localPosition, target.localPosition, Time.deltaTime * 5f);

            cg.alpha = Mathf.Lerp(cg.alpha, destAlpha, Time.deltaTime * 5f);
            instance.transform.localPosition = Vector3.Lerp(instance.transform.localPosition, Vector3.zero, Time.deltaTime * 5f);
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

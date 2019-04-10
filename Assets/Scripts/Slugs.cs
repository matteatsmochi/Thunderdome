using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slugs : MonoBehaviour
{
    public List<SlugsClass> SlugList;

    TwitchAPI tapi;
    Bot bot;

    
    int i;

    void Awake()
    {
        tapi = GameObject.Find("API Manager").GetComponent<TwitchAPI>();
        bot = GameObject.Find("Chat Bot").GetComponent<Bot>();
    }
    
    void Start()
    {
        i = 0;

        AddSlug("HeadstrongCreativeWafflePJSugar", "Default");
        AddSlug("PunchyHomelyHabaneroHeyGirl", "Default");
        AddSlug("AuspiciousTubularBunnyFUNgineer", "Default");
        AddSlug("CrypticFurryPeachYee", "Default");
        AddSlug("SourBlushingHedgehogOhMyDog", "Default");
        AddSlug("DistinctSpoopyNoodleCharlieBitMe", "Default");
        AddSlug("CarelessPlayfulVelociraptorHeyGuys", "Default");
        AddSlug("OnerousCorrectWrenchCeilingCat", "Default");
        AddSlug("RealAdorableLlamaSaltBae", "Default");
        AddSlug("TawdryExquisiteGoshawkAMPEnergyCherry", "Default");
        AddSlug("SmoothVainStapleAMPEnergy", "Default");
        AddSlug("DeliciousSuccessfulSnailCmonBruh", "Default");
        AddSlug("AbrasiveInterestingWhaleLitty", "Default");
        AddSlug("SuperSecretiveChimpanzeeBlargNaut", "Default");
        AddSlug("HonestCrackyCaribouTakeNRG", "Default");
        AddSlug("ImpossibleClumsyChickpeaTwitchRPG", "Default");
        AddSlug("AlertAbnegateHummingbirdArgieB8", "Default");
        AddSlug("BoxyLovelyKimchiOneHand", "Default");
        

        tapi.GetNewSlugs();
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            
            AddSlug("PunchyHomelyHabaneroHeyGirl", "Default");
        }
        
    }

    

    void SortSlugs()
    {
        for (int z = SlugList.Count - 1; z >= 0; z --)
        {
            if (SlugList[z].Spread <= -10)
            {
                SlugList.RemoveAt(z);
            }
        }
        
        
        SlugList.Sort(delegate(SlugsClass x, SlugsClass y)
        {
             return y.Spread.CompareTo(x.Spread);
        });

        i = 0;
    }

    public string NextSlug(int change)
    {
Restart:
        i++;
        if (i > SlugList.Count)
        {
            SortSlugs();
            goto Restart;
        }
        else
        {
            //UpdateSlugSpread(change);
            return SlugList[i - 1].SlugURL;
        }

    }

    public void AddSlug(string slug, string sb)
    {
        StartCoroutine(AddSlugThread(slug, sb));
    }

    IEnumerator AddSlugThread(string slug, string sb)
    {
        if (tapi.RealSlug(slug))
        {
            Tuple<string, int> result = tapi.GetSlugInfo(slug);
            string tempVODurl = result.Item1;
            int tempOffset = result.Item2;
            
            bool cleartoadd = true;
            if (SlugList.Count != 0)
            {
                for (int i = 0; i < SlugList.Count; i++)
                {
                    if (DuplicateSlug(tempVODurl, tempOffset, SlugList[i].VODurl, SlugList[i].offset))
                    {
                        cleartoadd = false;
                        break;
                    }
                }
            }

            if (cleartoadd)
            {
                SlugList.Add(new SlugsClass{SlugURL = slug, Spread = 5, SubmittedBy = sb, VODurl = tempVODurl, offset = tempOffset});
            }
        }

        yield return new WaitForSeconds(0);
    }


    bool DuplicateSlug(string VODurl1, int offset1, string VODurl2, int offset2)
    {
        if (VODurl1 == VODurl2)
        {
            if (Mathf.Abs(offset1 - offset2) < 180)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }      
    }

    void UpdateClock()
    {
        StartCoroutine(UpdateClockWait());
    }

    IEnumerator UpdateClockWait()
    {
        yield return new WaitForSeconds(5);
        string Clock = System.DateTime.Now.ToShortTimeString();
        Clock = Clock.Substring(0, Clock.Length - 3);
        Clock = Clock.Substring(Clock.Length - 2, 2);
        if (Clock == "00")
        {
            tapi.GetNewSlugs();
            bot.ClearVoteMinus();
        }

        UpdateClock();
    }

    void UpdateSlugSpread(int change)
    {
        if (SlugList.Count > 0)
        {
            int a = SlugList[i].Spread + change;
            string saveS = SlugList[i].SlugURL;
            string saveSB = SlugList[i].SubmittedBy;
            string saveVU = SlugList[i].VODurl;
            int saveO = SlugList[i].offset;
            
            SlugList.RemoveAt(i);

            SlugList.Insert(i, new SlugsClass{SlugURL = saveS, Spread = a, SubmittedBy = saveSB, VODurl = saveVU, offset = saveO});
        }
    }

    
}

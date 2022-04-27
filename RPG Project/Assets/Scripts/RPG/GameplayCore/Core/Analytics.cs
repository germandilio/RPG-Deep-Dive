using System;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using Unity.Services.Core;

namespace RPG.GameplayCore.Core
{
    public class Analytics : MonoBehaviour
    {
        async void Start()
        {
            try
            {
                await UnityServices.InitializeAsync();
                List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
            }
            catch (ConsentCheckException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
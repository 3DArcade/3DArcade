using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Arcade
{
    public class MoveCabsEditGamelistFilter : MonoBehaviour
    {
        public static List<ModelProperties> selectedGamelist = new List<ModelProperties>();
        public static List<ModelProperties> filteredSelectedGamelist = new List<ModelProperties>();

        public Dropdown emulators;
        public Dropdown genre;
        public Dropdown manufacturer;
        public Dropdown decade;
        public Dropdown original;
        public Dropdown available;
        public Dropdown screen;
        public Dropdown orientation;

        public InputField search;
        public MoveCabsEditGameProperties gameProperties;

        private string currentEmulatorSelection = "mame";
        private string currentGenreSelection = "All";
        private string currentManufacturerSelection = "All";
        private string currentDecadeSelection = "All";
        private string currentOriginalSelection = "All";
        private string currentAvailableSelection = "All";
        private string currentScreenSelection = "All";
        private string currentOrientationSelection = "All";

        private string currentSearchSelection = "";

        void Start()
        {
            print("1 maal is genoeg");
            emulators.onValueChanged.AddListener(delegate
            { DropdownValueChangedHandler(emulators); });
            genre.onValueChanged.AddListener(delegate
            { DropdownValueChangedHandler(genre); });
            manufacturer.onValueChanged.AddListener(delegate
            { DropdownValueChangedHandler(manufacturer); });
            decade.onValueChanged.AddListener(delegate
            { DropdownValueChangedHandler(decade); });
            original.onValueChanged.AddListener(delegate
            { DropdownValueChangedHandler(original); });
            available.onValueChanged.AddListener(delegate
            { DropdownValueChangedHandler(available); });
            screen.onValueChanged.AddListener(delegate
            { DropdownValueChangedHandler(screen); });
            orientation.onValueChanged.AddListener(delegate
            { DropdownValueChangedHandler(orientation); });
            search.onValueChanged.AddListener(delegate
            { SearchValueChangedHandler(search); });

            // Emulators
            if (ArcadeManager.emulatorsConfigurationList.Count > 0)
            {

                emulators.options.Clear();
                string selectedName = "";
                EmulatorConfiguration selectedEmulatorConfiguration = ArcadeManager.emulatorsConfigurationList[0];
                foreach (EmulatorConfiguration item in ArcadeManager.emulatorsConfigurationList)
                {
                    emulators.options.Add(new Dropdown.OptionData(item.emulator.descriptiveName));
                    if (item.emulator.id == "mame")
                    {
                        selectedName = item.emulator.descriptiveName;
                        selectedEmulatorConfiguration = item;
                    }
                }
                emulators.value = emulators.options.FindIndex(option => option.text == selectedName);
                SetupList(selectedEmulatorConfiguration.masterGamelist);
            }
        }

        private void SetupList(List<ModelProperties> masterGamelist)
        {
            selectedGamelist = masterGamelist;
            selectedGamelist = selectedGamelist.OrderBy(x => x.descriptiveName).ToList();
            FilterGamelist();
        }

        private void SetupFilters()
        {
            Dropdown dropdown = genre;
            List<string> itemsList = new List<string>();
            foreach (ModelProperties item in filteredSelectedGamelist)
            {
                string titem = Regex.Replace(item.genre.Split()[0], @"[^0-9a-zA-Z\ ]+", "").Trim();
                if (!itemsList.Contains(titem) && titem != "")
                {
                    itemsList.Add(titem);
                }
            }
            genre.RefreshShownValue();
            SetupDropDownList(genre, itemsList.OrderBy(x => x).ToList());
            itemsList = new List<string>();
            foreach (ModelProperties item in filteredSelectedGamelist)
            {
                string titem = item.manufacturer.Trim();
                if (!itemsList.Contains(titem) && titem != "")
                {
                    itemsList.Add(titem);
                }
            }
            SetupDropDownList(manufacturer, itemsList.OrderBy(x => x).ToList());
            manufacturer.RefreshShownValue();
            itemsList = new List<string> { "<70s", "70s", "80s", "90s", ">90s" };
            SetupDropDownList(decade, itemsList);
            decade.RefreshShownValue();
            itemsList = new List<string> { "Original", "Clone" };
            SetupDropDownList(original, itemsList);
            original.RefreshShownValue();
            itemsList = new List<string> { "Available", "Unavailable" };
            SetupDropDownList(available, itemsList);
            available.RefreshShownValue();
            itemsList = new List<string> { "Raster", "Vector" };
            SetupDropDownList(screen, itemsList);
            screen.RefreshShownValue();
            itemsList = new List<string> { "Horizontal", "Vertical" };
            SetupDropDownList(orientation, itemsList);
            orientation.RefreshShownValue();
        }

        private void SetupDropDownList(Dropdown dropdown, List<string> list)
        {
            dropdown.options.Clear();
            dropdown.options.Add(new Dropdown.OptionData("All"));
            foreach (string item in list)
            {
                dropdown.options.Add(new Dropdown.OptionData(item));
            }
            // dropdown.value = 0;
        }

        private void FilterGamelist()
        {
            filteredSelectedGamelist = selectedGamelist;
            //print("prelistcount " + filteredSelectedGamelist.Count + " " + currentGenreSelection + " " + currentManufacturerSelection);
            //filteredSelectedGamelist = filteredSelectedGamelist.Where(x => x.idParent.Trim() == "").ToList();
            //filteredSelectedGamelist = filteredSelectedGamelist.Where(x => x.runnable == true).ToList();
            filteredSelectedGamelist = filteredSelectedGamelist.Where(x => x.mature == false).ToList();
            if (currentSearchSelection != "")
            {
                if (currentSearchSelection.StartsWith("."))
                {
                    // Reset Filters
                    currentGenreSelection = "All";
                    genre.value = 0;
                    currentManufacturerSelection = "All";
                    manufacturer.value = 0;
                    currentDecadeSelection = "All";
                    decade.value = 0;
                    currentOriginalSelection = "All";
                    original.value = 0;
                    currentAvailableSelection = "All";
                    available.value = 0;
                    currentScreenSelection = "All";
                    screen.value = 0;
                    currentOrientationSelection = "All";
                    original.value = 0;
                    currentSearchSelection = "";
                    search.text = "";
                }
                else if (currentSearchSelection.StartsWith("/"))
                {
                    filteredSelectedGamelist = filteredSelectedGamelist.Where(x => x.descriptiveName.Trim().ToLower().Contains(currentSearchSelection.Replace("/", "").Trim().ToLower())).ToList();
                }
                else
                {
                    filteredSelectedGamelist = filteredSelectedGamelist.Where(x => x.descriptiveName.Trim().ToLower().StartsWith(currentSearchSelection.Trim().ToLower())).ToList();
                }
            }
            if (currentOriginalSelection != "All")
            {
                if (currentOriginalSelection == "Original")
                {
                    filteredSelectedGamelist = filteredSelectedGamelist.Where(x => x.idParent.Trim() == "").ToList();
                }
                if (currentOriginalSelection == "Clone")
                {
                    filteredSelectedGamelist = filteredSelectedGamelist.Where(x => x.idParent.Trim() != "").ToList();
                }
            }
            if (currentAvailableSelection != "All")
            {
                if (currentAvailableSelection == "Available")
                {
                    filteredSelectedGamelist = filteredSelectedGamelist.Where(x => x.available == true).ToList();
                }
                if (currentAvailableSelection == "UnAvailable")
                {
                    filteredSelectedGamelist = filteredSelectedGamelist.Where(x => x.available == false).ToList();
                }
            }
            if (currentGenreSelection != "All")
            {
                filteredSelectedGamelist = filteredSelectedGamelist.Where(x => x.genre.Contains(currentGenreSelection)).ToList();
            }
            if (currentManufacturerSelection != "All")
            {
                filteredSelectedGamelist = filteredSelectedGamelist.Where(x => x.manufacturer.Contains(currentManufacturerSelection)).ToList();
            }
            if (currentScreenSelection != "All")
            {
                print("screen is " + currentScreenSelection);
                if (currentScreenSelection == "Raster")
                {
                    filteredSelectedGamelist = filteredSelectedGamelist.Where(x => x.screen.Contains("raster")).ToList();
                }
                if (currentScreenSelection == "Vector")
                {
                    filteredSelectedGamelist = filteredSelectedGamelist.Where(x => x.screen.Contains("vector")).ToList();
                }
            }
            if (currentOrientationSelection != "All")
            {
                if (currentOrientationSelection == "Horizontal")
                {
                    filteredSelectedGamelist = filteredSelectedGamelist.Where(x => x.screen.Contains("horizontal")).ToList();
                }
                if (currentOrientationSelection == "Vertical")
                {
                    filteredSelectedGamelist = filteredSelectedGamelist.Where(x => x.screen.Contains("vertical")).ToList();
                }
            }
            if (currentDecadeSelection != "All")
            {
                if (currentDecadeSelection == "<70s")
                {
                    filteredSelectedGamelist = filteredSelectedGamelist.Where(x => (int.TryParse(x.year.Trim(), out int number) ? number < 1970 : false)).ToList();
                }
                if (currentDecadeSelection == "70s")
                {
                    filteredSelectedGamelist = filteredSelectedGamelist.Where(x => (int.TryParse(x.year.Trim(), out int number) ? number >= 1970 && number < 1980 : false)).ToList();
                }
                if (currentDecadeSelection == "80s")
                {
                    filteredSelectedGamelist = filteredSelectedGamelist.Where(x => (int.TryParse(x.year.Trim(), out int number) ? number >= 1980 && number < 1990 : false)).ToList();
                }
                if (currentDecadeSelection == "90s")
                {
                    filteredSelectedGamelist = filteredSelectedGamelist.Where(x => (int.TryParse(x.year.Trim(), out int number) ? number >= 1990 && number < 2000 : false)).ToList();
                }
                if (currentDecadeSelection == ">90s")
                {
                    filteredSelectedGamelist = filteredSelectedGamelist.Where(x => (int.TryParse(x.year.Trim(), out int number) ? number >= 2000 : false)).ToList();
                }
            }
            LoopScrollRect scrollRects = gameObject.GetComponentInChildren<LoopScrollRect>();
            LoopScrollRect ls = scrollRects;
            ls.totalCount = filteredSelectedGamelist.Count;
            ls.RefillCells();
            //print("listcount " + filteredSelectedGamelist.Count);
            SetupFilters();
        }

        private void SearchValueChangedHandler(InputField target)
        {
            Debug.Log("selectedsearch: " + target.text);
            if (target == search)
            {
                currentSearchSelection = target.text;
                FilterGamelist();
            }
        }

        private void DropdownValueChangedHandler(Dropdown target)
        {
            Debug.Log("selected: " + target.name + " " + target.value);

            if (target == emulators)
            {
                currentEmulatorSelection = target.options[target.value].text;
                List<EmulatorConfiguration> list = ArcadeManager.emulatorsConfigurationList.Where(x => x.emulator.descriptiveName.ToLower() == currentEmulatorSelection.ToLower()).ToList<EmulatorConfiguration>();
                if (list.Count > 0)
                {
                    SetupList(list[0].masterGamelist);
                }
            }

            if (target == genre)
            {
                print("options " + target.options.Count);
                currentGenreSelection = target.options[target.value].text;
            }
            if (target == manufacturer)
            {
                currentManufacturerSelection = target.options[target.value].text;
            }
            if (target == decade)
            {
                currentDecadeSelection = target.options[target.value].text;
            }
            if (target == original)
            {
                currentOriginalSelection = target.options[target.value].text;
            }
            if (target == available)
            {
                currentAvailableSelection = target.options[target.value].text;
            }
            if (target == screen)
            {
                currentScreenSelection = target.options[target.value].text;
            }
            if (target == orientation)
            {
                currentOrientationSelection = target.options[target.value].text;
            }
            FilterGamelist();

        }

        public ModelProperties GetSelectedGame()
        {
            ModelProperties modelProperties = new ModelProperties();

            return null;
        }

        public void SetSelectedGame(int index)
        {
            print("game sel is " + filteredSelectedGamelist[index].descriptiveName);
            gameProperties.SetGameProperties(filteredSelectedGamelist[index]);
            //   gameProperties.emulator.text = filteredSelectedGamelist[index].emulator;
            //   gameProperties.model.text = filteredSelectedGamelist[index].model;
        }
    }
}

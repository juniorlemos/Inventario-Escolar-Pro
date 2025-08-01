﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InventarioEscolar.Exceptions {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ResourceMessagesException {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourceMessagesException() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("InventarioEscolar.Exceptions.ResourceMessagesException", typeof(ResourceMessagesException).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error, the asset code must be a positive number..
        /// </summary>
        public static string ASSET_CODE_MUST_BE_POSITIVE {
            get {
                return ResourceManager.GetString("ASSET_CODE_MUST_BE_POSITIVE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Asset id invalid.
        /// </summary>
        public static string ASSET_ID_INVALID {
            get {
                return ResourceManager.GetString("ASSET_ID_INVALID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The asset does not belong to the user&apos;s school..
        /// </summary>
        public static string ASSET_NOT_BELONG_TO_SCHOOL {
            get {
                return ResourceManager.GetString("ASSET_NOT_BELONG_TO_SCHOOL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Asset not found.
        /// </summary>
        public static string ASSET_NOT_FOUND {
            get {
                return ResourceManager.GetString("ASSET_NOT_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Asset ID must be greater than 0..
        /// </summary>
        public static string ASSETMOVEMENT_ASSET_ID_INVALID {
            get {
                return ResourceManager.GetString("ASSETMOVEMENT_ASSET_ID_INVALID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Asset movement cancel reason cannot exceed 200 characters..
        /// </summary>
        public static string ASSETMOVEMENT_CANCELREASON_TOO_LONG {
            get {
                return ResourceManager.GetString("ASSETMOVEMENT_CANCELREASON_TOO_LONG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to From Room ID must be greater than 0..
        /// </summary>
        public static string ASSETMOVEMENT_FROM_ROOM_ID_INVALID {
            get {
                return ResourceManager.GetString("ASSETMOVEMENT_FROM_ROOM_ID_INVALID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Moved At date cannot be in the future..
        /// </summary>
        public static string ASSETMOVEMENT_MOVED_AT_INVALID {
            get {
                return ResourceManager.GetString("ASSETMOVEMENT_MOVED_AT_INVALID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Asset movement not found..
        /// </summary>
        public static string ASSETMOVEMENT_NOT_FOUND {
            get {
                return ResourceManager.GetString("ASSETMOVEMENT_NOT_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Asset movement responsible name cannot exceed 100 characters..
        /// </summary>
        public static string ASSETMOVEMENT_RESPONSIBLE_NAME_TOO_LONG {
            get {
                return ResourceManager.GetString("ASSETMOVEMENT_RESPONSIBLE_NAME_TOO_LONG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Asset movement same room..
        /// </summary>
        public static string ASSETMOVEMENT_SAME_ROOM {
            get {
                return ResourceManager.GetString("ASSETMOVEMENT_SAME_ROOM", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to To Room ID must be greater than 0..
        /// </summary>
        public static string ASSETMOVEMENT_TO_ROOM_ID_INVALID {
            get {
                return ResourceManager.GetString("ASSETMOVEMENT_TO_ROOM_ID_INVALID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The category description must not exceed 200 characters..
        /// </summary>
        public static string CATEGORY_DESCRIPTION_TOOLONG {
            get {
                return ResourceManager.GetString("CATEGORY_DESCRIPTION_TOOLONG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot delete category because it still has assets assigned..
        /// </summary>
        public static string CATEGORY_HAS_ASSETS {
            get {
                return ResourceManager.GetString("CATEGORY_HAS_ASSETS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Category id invalid.
        /// </summary>
        public static string CATEGORY_ID_INVALID {
            get {
                return ResourceManager.GetString("CATEGORY_ID_INVALID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Category name already exists.
        /// </summary>
        public static string CATEGORY_NAME_ALREADY_EXISTS {
            get {
                return ResourceManager.GetString("CATEGORY_NAME_ALREADY_EXISTS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The category name must not exceed 100 characters..
        /// </summary>
        public static string CATEGORY_NAME_TOOLONG {
            get {
                return ResourceManager.GetString("CATEGORY_NAME_TOOLONG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The category name must be at least 2 characters long..
        /// </summary>
        public static string CATEGORY_NAME_TOOSHORT {
            get {
                return ResourceManager.GetString("CATEGORY_NAME_TOOSHORT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The category does not belong to the user&apos;s school..
        /// </summary>
        public static string CATEGORY_NOT_BELONG_TO_SCHOOL {
            get {
                return ResourceManager.GetString("CATEGORY_NOT_BELONG_TO_SCHOOL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Category not found.
        /// </summary>
        public static string CATEGORY_NOT_FOUND {
            get {
                return ResourceManager.GetString("CATEGORY_NOT_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The conservation status of the asset is not recognized.
        /// </summary>
        public static string CONSERVATION_STATE_NOT_SUPPORTED_ {
            get {
                return ResourceManager.GetString("CONSERVATION_STATE_NOT_SUPPORTED;", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The category is mandatory and must be valid..
        /// </summary>
        public static string INVALID_CATEGORY {
            get {
                return ResourceManager.GetString("INVALID_CATEGORY", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid name. The name must be between 3 and 100 characters..
        /// </summary>
        public static string INVALID_NUMBER_NAME {
            get {
                return ResourceManager.GetString("INVALID_NUMBER_NAME", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The location room must be valid..
        /// </summary>
        public static string INVALID_ROOM_LOCALIZATION {
            get {
                return ResourceManager.GetString("INVALID_ROOM_LOCALIZATION", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid description. The description can have a maximum of 200 characters..
        /// </summary>
        public static string MAXIMUM_DESCRIPTION_NUMBER {
            get {
                return ResourceManager.GetString("MAXIMUM_DESCRIPTION_NUMBER", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid serie number. The serie number can have a maximum of 30 characters..
        /// </summary>
        public static string MAXIMUM_SERIE_NUMBER {
            get {
                return ResourceManager.GetString("MAXIMUM_SERIE_NUMBER", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The name cannot be empty..
        /// </summary>
        public static string NAME_EMPTY {
            get {
                return ResourceManager.GetString("NAME_EMPTY", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The field value must be a positive number
        ///ve number.
        /// </summary>
        public static string NEGATIVE_NUMBER {
            get {
                return ResourceManager.GetString("NEGATIVE_NUMBER", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Patrimony code already exists.
        /// </summary>
        public static string PATRIMONY_CODE_ALREADY_EXISTS_ {
            get {
                return ResourceManager.GetString("PATRIMONY_CODE_ALREADY_EXISTS ", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The room location building must not exceed 50 characters..
        /// </summary>
        public static string ROOMLOCATION_BUILDING_TOOLONG {
            get {
                return ResourceManager.GetString("ROOMLOCATION_BUILDING_TOOLONG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The room location description must not exceed 200 characters..
        /// </summary>
        public static string ROOMLOCATION_DESCRIPTION_TOOLONG {
            get {
                return ResourceManager.GetString("ROOMLOCATION_DESCRIPTION_TOOLONG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot delete room location because it still has assets assigned..
        /// </summary>
        public static string ROOMLOCATION_HAS_ASSETS {
            get {
                return ResourceManager.GetString("ROOMLOCATION_HAS_ASSETS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Room location id invalid.
        /// </summary>
        public static string ROOMLOCATION_ID_INVALID {
            get {
                return ResourceManager.GetString("ROOMLOCATION_ID_INVALID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Room location name already exists.
        /// </summary>
        public static string ROOMLOCATION_NAME_ALREADY_EXISTS {
            get {
                return ResourceManager.GetString("ROOMLOCATION_NAME_ALREADY_EXISTS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The room location name must not exceed 100 characters..
        /// </summary>
        public static string ROOMLOCATION_NAME_TOOLONG {
            get {
                return ResourceManager.GetString("ROOMLOCATION_NAME_TOOLONG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The room location name must be at least 2 characters long..
        /// </summary>
        public static string ROOMLOCATION_NAME_TOOSHORT {
            get {
                return ResourceManager.GetString("ROOMLOCATION_NAME_TOOSHORT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The room location does not belong to the user&apos;s school..
        /// </summary>
        public static string ROOMLOCATION_NOT_BELONG_TO_SCHOOL {
            get {
                return ResourceManager.GetString("ROOMLOCATION_NOT_BELONG_TO_SCHOOL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Room location not found..
        /// </summary>
        public static string ROOMLOCATION_NOT_FOUND {
            get {
                return ResourceManager.GetString("ROOMLOCATION_NOT_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Room location destination not found..
        /// </summary>
        public static string ROOMLOCATION_NOT_FOUND_DESTINATION {
            get {
                return ResourceManager.GetString("ROOMLOCATION_NOT_FOUND_DESTINATION", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Room location origin not found..
        /// </summary>
        public static string ROOMLOCATION_NOT_FOUND_ORIGIN {
            get {
                return ResourceManager.GetString("ROOMLOCATION_NOT_FOUND_ORIGIN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to School address already exists.
        /// </summary>
        public static string SCHOOL_ADDRESS_ALREADY_EXISTS {
            get {
                return ResourceManager.GetString("SCHOOL_ADDRESS_ALREADY_EXISTS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The school address must not exceed 100 characters..
        /// </summary>
        public static string SCHOOL_ADDRESS_TOOLONG {
            get {
                return ResourceManager.GetString("SCHOOL_ADDRESS_TOOLONG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The school city must not exceed 30 characters..
        /// </summary>
        public static string SCHOOL_CITY_TOOLONG {
            get {
                return ResourceManager.GetString("SCHOOL_CITY_TOOLONG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot delete the school because it still has associated rooms, assets, or categories..
        /// </summary>
        public static string SCHOOL_HAS_DEPENDENCIES {
            get {
                return ResourceManager.GetString("SCHOOL_HAS_DEPENDENCIES", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to School id invalid.
        /// </summary>
        public static string SCHOOL_ID_INVALID {
            get {
                return ResourceManager.GetString("SCHOOL_ID_INVALID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to School INEP already exists.
        /// </summary>
        public static string SCHOOL_INEP_ALREADY_EXISTS {
            get {
                return ResourceManager.GetString("SCHOOL_INEP_ALREADY_EXISTS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The school INEP must not exceed 20 characters..
        /// </summary>
        public static string SCHOOL_INEP_TOOLONG {
            get {
                return ResourceManager.GetString("SCHOOL_INEP_TOOLONG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to School name already exists.
        /// </summary>
        public static string SCHOOL_NAME_ALREADY_EXISTS {
            get {
                return ResourceManager.GetString("SCHOOL_NAME_ALREADY_EXISTS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The school name must not exceed 100 characters..
        /// </summary>
        public static string SCHOOL_NAME_TOOLONG {
            get {
                return ResourceManager.GetString("SCHOOL_NAME_TOOLONG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The school name must be at least 2 characters long..
        /// </summary>
        public static string SCHOOL_NAME_TOOSHORT {
            get {
                return ResourceManager.GetString("SCHOOL_NAME_TOOSHORT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to School not found.
        /// </summary>
        public static string SCHOOL_NOT_FOUND {
            get {
                return ResourceManager.GetString("SCHOOL_NOT_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown error..
        /// </summary>
        public static string UNKNOWN_ERROR {
            get {
                return ResourceManager.GetString("UNKNOWN_ERROR", resourceCulture);
            }
        }
    }
}

using System;

using ObjCRuntime;
using Foundation;
using UIKit;

using Karte.iOS.Variables;
using Karte.iOS.Core;

namespace Karte.iOS.Variables
{
    // @interface KRTVariable : NSObject
    [BaseType(typeof(NSObject))]
	[DisableDefaultCtor]
	interface KRTVariable
	{
		// @property (copy, nonatomic) NSString * _Nullable campaignId;
		[NullAllowed, Export("campaignId")]
		string CampaignId { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable shortenId;
		[NullAllowed, Export("shortenId")]
		string ShortenId { get; set; }

		// @property (copy, nonatomic) NSString * _Nonnull name;
		[Export("name")]
		string Name { get; set; }

		// @property (readonly, nonatomic) BOOL isDefined;
		[Export("isDefined")]
		bool IsDefined { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable string;
		[NullAllowed, Export("string")]
		string String { get; }

		// @property (readonly, copy, nonatomic) NSArray * _Nullable array;
		[NullAllowed, Export("array", ArgumentSemantic.Copy)]
		string[] Array { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,id> * _Nullable dictionary;
		[NullAllowed, Export("dictionary", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> Dictionary { get; }

		// -(instancetype _Nonnull)initWithName:(NSString * _Nonnull)name __attribute__((objc_designated_initializer));
		[Export("initWithName:")]
		[DesignatedInitializer]
		IntPtr Constructor(string name);

		// -(NSString * _Nonnull)stringWithDefaultValue:(NSString * _Nonnull)value __attribute__((warn_unused_result("")));
		[Export("stringWithDefaultValue:")]
		string StringWithDefaultValue(string value);

		// -(NSInteger)integerWithDefaultValue:(NSInteger)value __attribute__((warn_unused_result("")));
		[Export("integerWithDefaultValue:")]
		nint IntegerWithDefaultValue(nint value);

		// -(double)doubleWithDefaultValue:(double)value __attribute__((warn_unused_result("")));
		[Export("doubleWithDefaultValue:")]
		double DoubleWithDefaultValue(double value);

		// -(BOOL)boolWithDefaultValue:(BOOL)value __attribute__((warn_unused_result("")));
		[Export("boolWithDefaultValue:")]
		bool BoolWithDefaultValue(bool value);

		// -(NSArray * _Nonnull)arrayWithDefaultValue:(NSArray * _Nonnull)value __attribute__((warn_unused_result("")));
		[Export("arrayWithDefaultValue:")]
		string[] ArrayWithDefaultValue(string[] value);

		// -(NSDictionary<NSString *,id> * _Nonnull)dictionaryWithDefaultValue:(NSDictionary<NSString *,id> * _Nonnull)value __attribute__((warn_unused_result("")));
		[Export("dictionaryWithDefaultValue:")]
		NSDictionary<NSString, NSObject> DictionaryWithDefaultValue(NSDictionary<NSString, NSObject> value);
	}

	// @interface KRTVariables : NSObject
	[BaseType(typeof(NSObject))]
	interface KRTVariables
	{
		// +(void)fetchWithCompletion:(void (^ _Nullable)(BOOL))completion;
		[Static]
		[Export("fetchWithCompletion:")]
		void FetchWithCompletion([NullAllowed] Action<bool> completion);

		// +(KRTVariable * _Nonnull)variableForKey:(NSString * _Nonnull)key __attribute__((warn_unused_result("")));
		[Static]
		[Export("variableForKey:")]
		KRTVariable VariableForKey(string key);
    }

}


// =================================================================================================
// ADOBE SYSTEMS INCORPORATED
// Copyright 2006 Adobe Systems Incorporated
// All Rights Reserved
//
// NOTICE:  Adobe permits you to use, modify, and distribute this file in accordance with the terms
// of the Adobe license agreement accompanying it.
// =================================================================================================
using System;
using Com.Adobe.Xmp;
using Com.Adobe.Xmp.Options;
using Com.Adobe.Xmp.Properties;
using Sharpen;

namespace Com.Adobe.Xmp
{
	/// <summary>This class represents the set of XMP metadata as a DOM representation.</summary>
	/// <remarks>
	/// This class represents the set of XMP metadata as a DOM representation. It has methods to read and
	/// modify all kinds of properties, create an iterator over all properties and serialize the metadata
	/// to a String, byte-array or <code>OutputStream</code>.
	/// </remarks>
	/// <since>20.01.2006</since>
	public interface XMPMeta : ICloneable
	{
		// ---------------------------------------------------------------------------------------------
		// Basic property manipulation functions
		/// <summary>
		/// The property value getter-methods all take a property specification: the first two parameters
		/// are always the top level namespace URI (the &quot;schema&quot; namespace) and the basic name
		/// of the property being referenced.
		/// </summary>
		/// <remarks>
		/// The property value getter-methods all take a property specification: the first two parameters
		/// are always the top level namespace URI (the &quot;schema&quot; namespace) and the basic name
		/// of the property being referenced. See the introductory discussion of path expression usage
		/// for more information.
		/// <p>
		/// All of the functions return an object inherited from <code>PropertyBase</code> or
		/// <code>null</code> if the property does not exists. The result object contains the value of
		/// the property and option flags describing the property. Arrays and the non-leaf levels of
		/// nodes do not have values.
		/// <p>
		/// See
		/// <see cref="Com.Adobe.Xmp.Options.PropertyOptions"/>
		/// for detailed information about the options.
		/// <p>
		/// This is the simplest property getter, mainly for top level simple properties or after using
		/// the path composition functions in XMPPathFactory.
		/// </remarks>
		/// <param name="schemaNS">
		/// The namespace URI for the property. May be <code>null</code> or the empty
		/// string if the first component of the propName path contains a namespace prefix. The
		/// URI must be for a registered namespace.
		/// </param>
		/// <param name="propName">
		/// The name of the property. May be a general path expression, must not be
		/// <code>null</code> or the empty string. Using a namespace prefix on the first
		/// component is optional. If present without a schemaNS value then the prefix specifies
		/// the namespace. The prefix must be for a registered namespace. If both a schemaNS URI
		/// and propName prefix are present, they must be corresponding parts of a registered
		/// namespace.
		/// </param>
		/// <returns>
		/// Returns a <code>XMPProperty</code> containing the value and the options or
		/// <code>null</code> if the property does not exist.
		/// </returns>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		XMPProperty GetProperty(string schemaNS, string propName);

		/// <summary>Provides access to items within an array.</summary>
		/// <remarks>
		/// Provides access to items within an array. The index is passed as an integer, you need not
		/// worry about the path string syntax for array items, convert a loop index to a string, etc.
		/// </remarks>
		/// <param name="schemaNS">The namespace URI for the array. Has the same usage as in GetProperty.</param>
		/// <param name="arrayName">
		/// The name of the array. May be a general path expression, must not be
		/// <code>null</code> or the empty string. Has the same namespace prefix usage as
		/// propName in <code>GetProperty()</code>.
		/// </param>
		/// <param name="itemIndex">
		/// The index of the desired item. Arrays in XMP are indexed from 1. The
		/// constant
		/// <see cref="XMPConstConstants.ArrayLastItem"/>
		/// always refers to the last existing array
		/// item.
		/// </param>
		/// <returns>
		/// Returns a <code>XMPProperty</code> containing the value and the options or
		/// <code>null</code> if the property does not exist.
		/// </returns>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		XMPProperty GetArrayItem(string schemaNS, string arrayName, int itemIndex);

		/// <summary>Returns the number of items in the array.</summary>
		/// <param name="schemaNS">The namespace URI for the array. Has the same usage as in GetProperty.</param>
		/// <param name="arrayName">
		/// The name of the array. May be a general path expression, must not be
		/// <code>null</code> or the empty string. Has the same namespace prefix usage as
		/// propName in <code>GetProperty()</code>.
		/// </param>
		/// <returns>Returns the number of items in the array.</returns>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		int CountArrayItems(string schemaNS, string arrayName);

		/// <summary>Provides access to fields within a nested structure.</summary>
		/// <remarks>
		/// Provides access to fields within a nested structure. The namespace for the field is passed as
		/// a URI, you need not worry about the path string syntax.
		/// <p>
		/// The names of fields should be XML qualified names, that is within an XML namespace. The path
		/// syntax for a qualified name uses the namespace prefix. This is unreliable since the prefix is
		/// never guaranteed. The URI is the formal name, the prefix is just a local shorthand in a given
		/// sequence of XML text.
		/// </remarks>
		/// <param name="schemaNS">The namespace URI for the struct. Has the same usage as in GetProperty.</param>
		/// <param name="structName">
		/// The name of the struct. May be a general path expression, must not be
		/// <code>null</code> or the empty string. Has the same namespace prefix usage as
		/// propName in <code>GetProperty()</code>.
		/// </param>
		/// <param name="fieldNS">
		/// The namespace URI for the field. Has the same URI and prefix usage as the
		/// schemaNS parameter.
		/// </param>
		/// <param name="fieldName">
		/// The name of the field. Must be a single XML name, must not be
		/// <code>null</code> or the empty string. Has the same namespace prefix usage as the
		/// structName parameter.
		/// </param>
		/// <returns>
		/// Returns a <code>XMPProperty</code> containing the value and the options or
		/// <code>null</code> if the property does not exist. Arrays and non-leaf levels of
		/// structs do not have values.
		/// </returns>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		XMPProperty GetStructField(string schemaNS, string structName, string fieldNS, string fieldName);

		/// <summary>Provides access to a qualifier attached to a property.</summary>
		/// <remarks>
		/// Provides access to a qualifier attached to a property. The namespace for the qualifier is
		/// passed as a URI, you need not worry about the path string syntax. In many regards qualifiers
		/// are like struct fields. See the introductory discussion of qualified properties for more
		/// information.
		/// <p>
		/// The names of qualifiers should be XML qualified names, that is within an XML namespace. The
		/// path syntax for a qualified name uses the namespace prefix. This is unreliable since the
		/// prefix is never guaranteed. The URI is the formal name, the prefix is just a local shorthand
		/// in a given sequence of XML text.
		/// <p>
		/// <em>Note:</em> Qualifiers are only supported for simple leaf properties at this time.
		/// </remarks>
		/// <param name="schemaNS">The namespace URI for the struct. Has the same usage as in GetProperty.</param>
		/// <param name="propName">
		/// The name of the property to which the qualifier is attached. May be a general
		/// path expression, must not be <code>null</code> or the empty string. Has the same
		/// namespace prefix usage as in <code>GetProperty()</code>.
		/// </param>
		/// <param name="qualNS">
		/// The namespace URI for the qualifier. Has the same URI and prefix usage as the
		/// schemaNS parameter.
		/// </param>
		/// <param name="qualName">
		/// The name of the qualifier. Must be a single XML name, must not be
		/// <code>null</code> or the empty string. Has the same namespace prefix usage as the
		/// propName parameter.
		/// </param>
		/// <returns>
		/// Returns a <code>XMPProperty</code> containing the value and the options of the
		/// qualifier or <code>null</code> if the property does not exist. The name of the
		/// qualifier must be a single XML name, must not be <code>null</code> or the empty
		/// string. Has the same namespace prefix usage as the propName parameter.
		/// <p>
		/// The value of the qualifier is only set if it has one (Arrays and non-leaf levels of
		/// structs do not have values).
		/// </returns>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		XMPProperty GetQualifier(string schemaNS, string propName, string qualNS, string qualName);

		// ---------------------------------------------------------------------------------------------
		// Functions for setting property values
		/// <summary>
		/// The property value <code>setters</code> all take a property specification, their
		/// differences are in the form of this.
		/// </summary>
		/// <remarks>
		/// The property value <code>setters</code> all take a property specification, their
		/// differences are in the form of this. The first two parameters are always the top level
		/// namespace URI (the <code>schema</code> namespace) and the basic name of the property being
		/// referenced. See the introductory discussion of path expression usage for more information.
		/// <p>
		/// All of the functions take a string value for the property and option flags describing the
		/// property. The value must be Unicode in UTF-8 encoding. Arrays and non-leaf levels of structs
		/// do not have values. Empty arrays and structs may be created using appropriate option flags.
		/// All levels of structs that is assigned implicitly are created if necessary. appendArayItem
		/// implicitly creates the named array if necessary.
		/// <p>
		/// See
		/// <see cref="Com.Adobe.Xmp.Options.PropertyOptions"/>
		/// for detailed information about the options.
		/// <p>
		/// This is the simplest property setter, mainly for top level simple properties or after using
		/// the path composition functions in
		/// <see cref="XMPPathFactory"/>
		/// .
		/// </remarks>
		/// <param name="schemaNS">The namespace URI for the property. Has the same usage as in GetProperty.</param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <param name="propValue">
		/// the value for the property (only leaf properties have a value).
		/// Arrays and non-leaf levels of structs do not have values.
		/// Must be <code>null</code> if the value is not relevant.<br/>
		/// The value is automatically detected: Boolean, Integer, Long, Double, XMPDateTime and
		/// byte[] are handled, on all other <code>ToString()</code> is called.
		/// </param>
		/// <param name="options">Option flags describing the property. See the earlier description.</param>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetProperty(string schemaNS, string propName, object propValue, PropertyOptions options);

		/// <seealso cref="SetProperty(string, string, object, Com.Adobe.Xmp.Options.PropertyOptions)"/>
		/// <param name="schemaNS">The namespace URI</param>
		/// <param name="propName">The name of the property</param>
		/// <param name="propValue">the value for the property</param>
		/// <exception cref="XMPException">Wraps all errors and exceptions</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetProperty(string schemaNS, string propName, object propValue);

		/// <summary>Replaces an item within an array.</summary>
		/// <remarks>
		/// Replaces an item within an array. The index is passed as an integer, you need not worry about
		/// the path string syntax for array items, convert a loop index to a string, etc. The array
		/// passed must already exist. In normal usage the selected array item is modified. A new item is
		/// automatically appended if the index is the array size plus 1.
		/// </remarks>
		/// <param name="schemaNS">The namespace URI for the array. Has the same usage as in GetProperty.</param>
		/// <param name="arrayName">
		/// The name of the array. May be a general path expression, must not be
		/// <code>null</code> or the empty string. Has the same namespace prefix usage as
		/// propName in GetProperty.
		/// </param>
		/// <param name="itemIndex">
		/// The index of the desired item. Arrays in XMP are indexed from 1. To address
		/// the last existing item, use
		/// <see cref="CountArrayItems(string, string)"/>
		/// to find
		/// out the length of the array.
		/// </param>
		/// <param name="itemValue">
		/// the new value of the array item. Has the same usage as propValue in
		/// <code>SetProperty()</code>.
		/// </param>
		/// <param name="options">the set options for the item.</param>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetArrayItem(string schemaNS, string arrayName, int itemIndex, string itemValue, PropertyOptions options);

		/// <seealso cref="SetArrayItem(string, string, int, string, Com.Adobe.Xmp.Options.PropertyOptions)"/>
		/// <param name="schemaNS">The namespace URI</param>
		/// <param name="arrayName">The name of the array</param>
		/// <param name="itemIndex">The index to insert the new item</param>
		/// <param name="itemValue">the new value of the array item</param>
		/// <exception cref="XMPException">Wraps all errors and exceptions</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetArrayItem(string schemaNS, string arrayName, int itemIndex, string itemValue);

		/// <summary>Inserts an item into an array previous to the given index.</summary>
		/// <remarks>
		/// Inserts an item into an array previous to the given index. The index is passed as an integer,
		/// you need not worry about the path string syntax for array items, convert a loop index to a
		/// string, etc. The array passed must already exist. In normal usage the selected array item is
		/// modified. A new item is automatically appended if the index is the array size plus 1.
		/// </remarks>
		/// <param name="schemaNS">The namespace URI for the array. Has the same usage as in GetProperty.</param>
		/// <param name="arrayName">
		/// The name of the array. May be a general path expression, must not be
		/// <code>null</code> or the empty string. Has the same namespace prefix usage as
		/// propName in GetProperty.
		/// </param>
		/// <param name="itemIndex">
		/// The index to insert the new item. Arrays in XMP are indexed from 1. Use
        /// <code>XMPConstConstants.ArrayLastItem</code> to append items.
		/// </param>
		/// <param name="itemValue">
		/// the new value of the array item. Has the same usage as
		/// propValue in <code>SetProperty()</code>.
		/// </param>
		/// <param name="options">the set options that decide about the kind of the node.</param>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void InsertArrayItem(string schemaNS, string arrayName, int itemIndex, string itemValue, PropertyOptions options);

		/// <seealso cref="InsertArrayItem(string, string, int, string, Com.Adobe.Xmp.Options.PropertyOptions)"/>
		/// <param name="schemaNS">The namespace URI for the array</param>
		/// <param name="arrayName">The name of the array</param>
		/// <param name="itemIndex">The index to insert the new item</param>
		/// <param name="itemValue">the value of the array item</param>
		/// <exception cref="XMPException">Wraps all errors and exceptions</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void InsertArrayItem(string schemaNS, string arrayName, int itemIndex, string itemValue);

		/// <summary>Simplifies the construction of an array by not requiring that you pre-create an empty array.</summary>
		/// <remarks>
		/// Simplifies the construction of an array by not requiring that you pre-create an empty array.
		/// The array that is assigned is created automatically if it does not yet exist. Each call to
		/// AppendArrayItem() appends an item to the array. The corresponding parameters have the same
		/// use as SetArrayItem(). The arrayOptions parameter is used to specify what kind of array. If
		/// the array exists, it must have the specified form.
		/// </remarks>
		/// <param name="schemaNS">The namespace URI for the array. Has the same usage as in GetProperty.</param>
		/// <param name="arrayName">
		/// The name of the array. May be a general path expression, must not be null or
		/// the empty string. Has the same namespace prefix usage as propPath in GetProperty.
		/// </param>
		/// <param name="arrayOptions">
		/// Option flags describing the array form. The only valid options are
		/// <ul>
		/// <li>
		/// <see cref="Com.Adobe.Xmp.Options.PropertyOptions.Array"/>
		/// ,
		/// <li>
		/// <see cref="Com.Adobe.Xmp.Options.PropertyOptions.ArrayOrdered"/>
		/// ,
		/// <li>
		/// <see cref="Com.Adobe.Xmp.Options.PropertyOptions.ArrayAlternate"/>
		/// or
		/// <li>
		/// <see cref="Com.Adobe.Xmp.Options.PropertyOptions.ArrayAltText"/>
		/// .
		/// </ul>
		/// <em>Note:</em> the array options only need to be provided if the array is not
		/// already existing, otherwise you can set them to <code>null</code> or use
		/// <see cref="AppendArrayItem(string, string, string)"/>
		/// .
		/// </param>
		/// <param name="itemValue">the value of the array item. Has the same usage as propValue in GetProperty.</param>
		/// <param name="itemOptions">
		/// Option flags describing the item to append (
		/// <see cref="Com.Adobe.Xmp.Options.PropertyOptions"/>
		/// )
		/// </param>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void AppendArrayItem(string schemaNS, string arrayName, PropertyOptions arrayOptions, string itemValue, PropertyOptions itemOptions);

		/// <seealso cref="AppendArrayItem(string, string, Com.Adobe.Xmp.Options.PropertyOptions, string, Com.Adobe.Xmp.Options.PropertyOptions)"/>
		/// <param name="schemaNS">The namespace URI for the array</param>
		/// <param name="arrayName">The name of the array</param>
		/// <param name="itemValue">the value of the array item</param>
		/// <exception cref="XMPException">Wraps all errors and exceptions</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void AppendArrayItem(string schemaNS, string arrayName, string itemValue);

		/// <summary>Provides access to fields within a nested structure.</summary>
		/// <remarks>
		/// Provides access to fields within a nested structure. The namespace for the field is passed as
		/// a URI, you need not worry about the path string syntax. The names of fields should be XML
		/// qualified names, that is within an XML namespace. The path syntax for a qualified name uses
		/// the namespace prefix, which is unreliable because the prefix is never guaranteed. The URI is
		/// the formal name, the prefix is just a local shorthand in a given sequence of XML text.
		/// </remarks>
		/// <param name="schemaNS">The namespace URI for the struct. Has the same usage as in GetProperty.</param>
		/// <param name="structName">
		/// The name of the struct. May be a general path expression, must not be null
		/// or the empty string. Has the same namespace prefix usage as propName in GetProperty.
		/// </param>
		/// <param name="fieldNS">
		/// The namespace URI for the field. Has the same URI and prefix usage as the
		/// schemaNS parameter.
		/// </param>
		/// <param name="fieldName">
		/// The name of the field. Must be a single XML name, must not be null or the
		/// empty string. Has the same namespace prefix usage as the structName parameter.
		/// </param>
		/// <param name="fieldValue">
		/// the value of thefield, if the field has a value.
		/// Has the same usage as propValue in GetProperty.
		/// </param>
		/// <param name="options">Option flags describing the field. See the earlier description.</param>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetStructField(string schemaNS, string structName, string fieldNS, string fieldName, string fieldValue, PropertyOptions options);

		/// <seealso cref="SetStructField(string, string, string, string, string, Com.Adobe.Xmp.Options.PropertyOptions)"/>
		/// <param name="schemaNS">The namespace URI for the struct</param>
		/// <param name="structName">The name of the struct</param>
		/// <param name="fieldNS">The namespace URI for the field</param>
		/// <param name="fieldName">The name of the field</param>
		/// <param name="fieldValue">the value of the field</param>
		/// <exception cref="XMPException">Wraps all errors and exceptions</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetStructField(string schemaNS, string structName, string fieldNS, string fieldName, string fieldValue);

		/// <summary>Provides access to a qualifier attached to a property.</summary>
		/// <remarks>
		/// Provides access to a qualifier attached to a property. The namespace for the qualifier is
		/// passed as a URI, you need not worry about the path string syntax. In many regards qualifiers
		/// are like struct fields. See the introductory discussion of qualified properties for more
		/// information. The names of qualifiers should be XML qualified names, that is within an XML
		/// namespace. The path syntax for a qualified name uses the namespace prefix, which is
		/// unreliable because the prefix is never guaranteed. The URI is the formal name, the prefix is
		/// just a local shorthand in a given sequence of XML text. The property the qualifier
		/// will be attached has to exist.
		/// </remarks>
		/// <param name="schemaNS">The namespace URI for the struct. Has the same usage as in GetProperty.</param>
		/// <param name="propName">
		/// The name of the property to which the qualifier is attached. Has the same
		/// usage as in GetProperty.
		/// </param>
		/// <param name="qualNS">
		/// The namespace URI for the qualifier. Has the same URI and prefix usage as the
		/// schemaNS parameter.
		/// </param>
		/// <param name="qualName">
		/// The name of the qualifier. Must be a single XML name, must not be
		/// <code>null</code> or the empty string. Has the same namespace prefix usage as the
		/// propName parameter.
		/// </param>
		/// <param name="qualValue">
		/// A pointer to the <code>null</code> terminated UTF-8 string that is the
		/// value of the qualifier, if the qualifier has a value. Has the same usage as propValue
		/// in GetProperty.
		/// </param>
		/// <param name="options">Option flags describing the qualifier. See the earlier description.</param>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetQualifier(string schemaNS, string propName, string qualNS, string qualName, string qualValue, PropertyOptions options);

		/// <seealso cref="SetQualifier(string, string, string, string, string, Com.Adobe.Xmp.Options.PropertyOptions)"/>
		/// <param name="schemaNS">The namespace URI for the struct</param>
		/// <param name="propName">The name of the property to which the qualifier is attached</param>
		/// <param name="qualNS">The namespace URI for the qualifier</param>
		/// <param name="qualName">The name of the qualifier</param>
		/// <param name="qualValue">the value of the qualifier</param>
		/// <exception cref="XMPException">Wraps all errors and exceptions</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetQualifier(string schemaNS, string propName, string qualNS, string qualName, string qualValue);

		// ---------------------------------------------------------------------------------------------
		// Functions for deleting and detecting properties. These should be obvious from the
		// descriptions of the getters and setters.
		/// <summary>Deletes the given XMP subtree rooted at the given property.</summary>
		/// <remarks>
		/// Deletes the given XMP subtree rooted at the given property. It is not an error if the
		/// property does not exist.
		/// </remarks>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="propName">The name of the property. Has the same usage as in GetProperty.</param>
		void DeleteProperty(string schemaNS, string propName);

		/// <summary>Deletes the given XMP subtree rooted at the given array item.</summary>
		/// <remarks>
		/// Deletes the given XMP subtree rooted at the given array item. It is not an error if the array
		/// item does not exist.
		/// </remarks>
		/// <param name="schemaNS">The namespace URI for the array. Has the same usage as in GetProperty.</param>
		/// <param name="arrayName">
		/// The name of the array. May be a general path expression, must not be
		/// <code>null</code> or the empty string. Has the same namespace prefix usage as
		/// propName in <code>GetProperty()</code>.
		/// </param>
		/// <param name="itemIndex">
		/// The index of the desired item. Arrays in XMP are indexed from 1. The
        /// constant <code>XMPConstConstants.ArrayLastItem</code> always refers to the last
		/// existing array item.
		/// </param>
		void DeleteArrayItem(string schemaNS, string arrayName, int itemIndex);

		/// <summary>Deletes the given XMP subtree rooted at the given struct field.</summary>
		/// <remarks>
		/// Deletes the given XMP subtree rooted at the given struct field. It is not an error if the
		/// field does not exist.
		/// </remarks>
		/// <param name="schemaNS">
		/// The namespace URI for the struct. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="structName">
		/// The name of the struct. May be a general path expression, must not be
		/// <code>null</code> or the empty string. Has the same namespace prefix usage as
		/// propName in GetProperty.
		/// </param>
		/// <param name="fieldNS">
		/// The namespace URI for the field. Has the same URI and prefix usage as the
		/// schemaNS parameter.
		/// </param>
		/// <param name="fieldName">
		/// The name of the field. Must be a single XML name, must not be
		/// <code>null</code> or the empty string. Has the same namespace prefix usage as the
		/// structName parameter.
		/// </param>
		void DeleteStructField(string schemaNS, string structName, string fieldNS, string fieldName);

		/// <summary>Deletes the given XMP subtree rooted at the given qualifier.</summary>
		/// <remarks>
		/// Deletes the given XMP subtree rooted at the given qualifier. It is not an error if the
		/// qualifier does not exist.
		/// </remarks>
		/// <param name="schemaNS">
		/// The namespace URI for the struct. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property to which the qualifier is attached. Has the same
		/// usage as in GetProperty.
		/// </param>
		/// <param name="qualNS">
		/// The namespace URI for the qualifier. Has the same URI and prefix usage as the
		/// schemaNS parameter.
		/// </param>
		/// <param name="qualName">
		/// The name of the qualifier. Must be a single XML name, must not be
		/// <code>null</code> or the empty string. Has the same namespace prefix usage as the
		/// propName parameter.
		/// </param>
		void DeleteQualifier(string schemaNS, string propName, string qualNS, string qualName);

		/// <summary>Returns whether the property exists.</summary>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <returns>Returns true if the property exists.</returns>
		bool DoesPropertyExist(string schemaNS, string propName);

		/// <summary>Tells if the array item exists.</summary>
		/// <param name="schemaNS">
		/// The namespace URI for the array. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="arrayName">
		/// The name of the array. May be a general path expression, must not be
		/// <code>null</code> or the empty string. Has the same namespace prefix usage as
		/// propName in <code>GetProperty()</code>.
		/// </param>
		/// <param name="itemIndex">
		/// The index of the desired item. Arrays in XMP are indexed from 1. The
        /// constant <code>XMPConstConstants.ArrayLastItem</code> always refers to the last
		/// existing array item.
		/// </param>
		/// <returns>Returns <code>true</code> if the array exists, <code>false</code> otherwise.</returns>
		bool DoesArrayItemExist(string schemaNS, string arrayName, int itemIndex);

		/// <summary>DoesStructFieldExist tells if the struct field exists.</summary>
		/// <param name="schemaNS">
		/// The namespace URI for the struct. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="structName">
		/// The name of the struct. May be a general path expression, must not be
		/// <code>null</code> or the empty string. Has the same namespace prefix usage as
		/// propName in <code>GetProperty()</code>.
		/// </param>
		/// <param name="fieldNS">
		/// The namespace URI for the field. Has the same URI and prefix usage as the
		/// schemaNS parameter.
		/// </param>
		/// <param name="fieldName">
		/// The name of the field. Must be a single XML name, must not be
		/// <code>null</code> or the empty string. Has the same namespace prefix usage as the
		/// structName parameter.
		/// </param>
		/// <returns>Returns true if the field exists.</returns>
		bool DoesStructFieldExist(string schemaNS, string structName, string fieldNS, string fieldName);

		/// <summary>DoesQualifierExist tells if the qualifier exists.</summary>
		/// <param name="schemaNS">
		/// The namespace URI for the struct. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property to which the qualifier is attached. Has the same
		/// usage as in <code>GetProperty()</code>.
		/// </param>
		/// <param name="qualNS">
		/// The namespace URI for the qualifier. Has the same URI and prefix usage as the
		/// schemaNS parameter.
		/// </param>
		/// <param name="qualName">
		/// The name of the qualifier. Must be a single XML name, must not be
		/// <code>null</code> or the empty string. Has the same namespace prefix usage as the
		/// propName parameter.
		/// </param>
		/// <returns>Returns true if the qualifier exists.</returns>
		bool DoesQualifierExist(string schemaNS, string propName, string qualNS, string qualName);

		// ---------------------------------------------------------------------------------------------
		// Specialized Get and Set functions
		/// <summary>
		/// These functions provide convenient support for localized text properties, including a number
		/// of special and obscure aspects.
		/// </summary>
		/// <remarks>
		/// These functions provide convenient support for localized text properties, including a number
		/// of special and obscure aspects. Localized text properties are stored in alt-text arrays. They
		/// allow multiple concurrent localizations of a property value, for example a document title or
		/// copyright in several languages. The most important aspect of these functions is that they
		/// select an appropriate array item based on one or two RFC 3066 language tags. One of these
		/// languages, the "specific" language, is preferred and selected if there is an exact match. For
		/// many languages it is also possible to define a "generic" language that may be used if there
		/// is no specific language match. The generic language must be a valid RFC 3066 primary subtag,
		/// or the empty string. For example, a specific language of "en-US" should be used in the US,
		/// and a specific language of "en-UK" should be used in England. It is also appropriate to use
		/// "en" as the generic language in each case. If a US document goes to England, the "en-US"
		/// title is selected by using the "en" generic language and the "en-UK" specific language. It is
		/// considered poor practice, but allowed, to pass a specific language that is just an RFC 3066
		/// primary tag. For example "en" is not a good specific language, it should only be used as a
		/// generic language. Passing "i" or "x" as the generic language is also considered poor practice
		/// but allowed. Advice from the W3C about the use of RFC 3066 language tags can be found at:
		/// http://www.w3.org/International/articles/language-tags/
		/// <p>
		/// <em>Note:</em> RFC 3066 language tags must be treated in a case insensitive manner. The XMP
		/// Toolkit does this by normalizing their capitalization:
		/// <ul>
		/// <li> The primary subtag is lower case, the suggested practice of ISO 639.
		/// <li> All 2 letter secondary subtags are upper case, the suggested practice of ISO 3166.
		/// <li> All other subtags are lower case. The XMP specification defines an artificial language,
		/// <li>"x-default", that is used to explicitly denote a default item in an alt-text array.
		/// </ul>
		/// The XMP toolkit normalizes alt-text arrays such that the x-default item is the first item.
		/// The SetLocalizedText function has several special features related to the x-default item, see
		/// its description for details. The selection of the array item is the same for GetLocalizedText
		/// and SetLocalizedText:
		/// <ul>
		/// <li> Look for an exact match with the specific language.
		/// <li> If a generic language is given, look for a partial match.
		/// <li> Look for an x-default item.
		/// <li> Choose the first item.
		/// </ul>
		/// A partial match with the generic language is where the start of the item's language matches
		/// the generic string and the next character is '-'. An exact match is also recognized as a
		/// degenerate case. It is fine to pass x-default as the specific language. In this case,
		/// selection of an x-default item is an exact match by the first rule, not a selection by the
		/// 3rd rule. The last 2 rules are fallbacks used when the specific and generic languages fail to
		/// produce a match. <code>getLocalizedText</code> returns information about a selected item in
		/// an alt-text array. The array item is selected according to the rules given above.
		/// <em>Note:</em> In a future version of this API a method
		/// using Java <code>java.lang.Locale</code> will be added.
		/// </remarks>
		/// <param name="schemaNS">
		/// The namespace URI for the alt-text array. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="altTextName">
		/// The name of the alt-text array. May be a general path expression, must not
		/// be <code>null</code> or the empty string. Has the same namespace prefix usage as
		/// propName in <code>GetProperty()</code>.
		/// </param>
		/// <param name="genericLang">
		/// The name of the generic language as an RFC 3066 primary subtag. May be
		/// <code>null</code> or the empty string if no generic language is wanted.
		/// </param>
		/// <param name="specificLang">
		/// The name of the specific language as an RFC 3066 tag. Must not be
		/// <code>null</code> or the empty string.
		/// </param>
		/// <returns>
		/// Returns an <code>XMPProperty</code> containing the value, the actual language and
		/// the options if an appropriate alternate collection item exists, <code>null</code>
		/// if the property.
		/// does not exist.
		/// </returns>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		XMPProperty GetLocalizedText(string schemaNS, string altTextName, string genericLang, string specificLang);

		/// <summary>Modifies the value of a selected item in an alt-text array.</summary>
		/// <remarks>
		/// Modifies the value of a selected item in an alt-text array. Creates an appropriate array item
		/// if necessary, and handles special cases for the x-default item. If the selected item is from
		/// a match with the specific language, the value of that item is modified. If the existing value
		/// of that item matches the existing value of the x-default item, the x-default item is also
		/// modified. If the array only has 1 existing item (which is not x-default), an x-default item
		/// is added with the given value. If the selected item is from a match with the generic language
		/// and there are no other generic matches, the value of that item is modified. If the existing
		/// value of that item matches the existing value of the x-default item, the x-default item is
		/// also modified. If the array only has 1 existing item (which is not x-default), an x-default
		/// item is added with the given value. If the selected item is from a partial match with the
		/// generic language and there are other partial matches, a new item is created for the specific
		/// language. The x-default item is not modified. If the selected item is from the last 2 rules
		/// then a new item is created for the specific language. If the array only had an x-default
		/// item, the x-default item is also modified. If the array was empty, items are created for the
		/// specific language and x-default.
		/// <em>Note:</em> In a future version of this API a method
		/// using Java <code>java.lang.Locale</code> will be added.
		/// </remarks>
		/// <param name="schemaNS">
		/// The namespace URI for the alt-text array. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="altTextName">
		/// The name of the alt-text array. May be a general path expression, must not
		/// be <code>null</code> or the empty string. Has the same namespace prefix usage as
		/// propName in <code>GetProperty()</code>.
		/// </param>
		/// <param name="genericLang">
		/// The name of the generic language as an RFC 3066 primary subtag. May be
		/// <code>null</code> or the empty string if no generic language is wanted.
		/// </param>
		/// <param name="specificLang">
		/// The name of the specific language as an RFC 3066 tag. Must not be
		/// <code>null</code> or the empty string.
		/// </param>
		/// <param name="itemValue">
		/// A pointer to the <code>null</code> terminated UTF-8 string that is the new
		/// value for the appropriate array item.
		/// </param>
		/// <param name="options">Option flags, none are defined at present.</param>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetLocalizedText(string schemaNS, string altTextName, string genericLang, string specificLang, string itemValue, PropertyOptions options);

		/// <seealso cref="SetLocalizedText(string, string, string, string, string, Com.Adobe.Xmp.Options.PropertyOptions)"/>
		/// <param name="schemaNS">The namespace URI for the alt-text array</param>
		/// <param name="altTextName">The name of the alt-text array</param>
		/// <param name="genericLang">The name of the generic language</param>
		/// <param name="specificLang">The name of the specific language</param>
		/// <param name="itemValue">the new value for the appropriate array item</param>
		/// <exception cref="XMPException">Wraps all errors and exceptions</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetLocalizedText(string schemaNS, string altTextName, string genericLang, string specificLang, string itemValue);

		// ---------------------------------------------------------------------------------------------
		// Functions accessing properties as binary values.
		/// <summary>
		/// These are very similar to <code>GetProperty()</code> and <code>SetProperty()</code> above,
		/// but the value is returned or provided in a literal form instead of as a UTF-8 string.
		/// </summary>
		/// <remarks>
		/// These are very similar to <code>GetProperty()</code> and <code>SetProperty()</code> above,
		/// but the value is returned or provided in a literal form instead of as a UTF-8 string.
		/// The path composition functions in <code>XMPPathFactory</code> may be used to compose an path
		/// expression for fields in nested structures, items in arrays, or qualifiers.
		/// </remarks>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <returns>
		/// Returns a <code>Boolean</code> value or <code>null</code>
		/// if the property does not exist.
		/// </returns>
		/// <exception cref="XMPException">
		/// Wraps all exceptions that may occur,
		/// especially conversion errors.
		/// </exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		bool GetPropertyBoolean(string schemaNS, string propName);

		/// <summary>Convenience method to retrieve the literal value of a property.</summary>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <returns>
		/// Returns an <code>Integer</code> value or <code>null</code>
		/// if the property does not exist.
		/// </returns>
		/// <exception cref="XMPException">
		/// Wraps all exceptions that may occur,
		/// especially conversion errors.
		/// </exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		int GetPropertyInteger(string schemaNS, string propName);

		/// <summary>Convenience method to retrieve the literal value of a property.</summary>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <returns>
		/// Returns a <code>Long</code> value or <code>null</code>
		/// if the property does not exist.
		/// </returns>
		/// <exception cref="XMPException">
		/// Wraps all exceptions that may occur,
		/// especially conversion errors.
		/// </exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		long GetPropertyLong(string schemaNS, string propName);

		/// <summary>Convenience method to retrieve the literal value of a property.</summary>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <returns>
		/// Returns a <code>Double</code> value or <code>null</code>
		/// if the property does not exist.
		/// </returns>
		/// <exception cref="XMPException">
		/// Wraps all exceptions that may occur,
		/// especially conversion errors.
		/// </exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		double GetPropertyDouble(string schemaNS, string propName);

		/// <summary>Convenience method to retrieve the literal value of a property.</summary>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <returns>
		/// Returns a <code>XMPDateTime</code>-object or <code>null</code>
		/// if the property does not exist.
		/// </returns>
		/// <exception cref="XMPException">
		/// Wraps all exceptions that may occur,
		/// especially conversion errors.
		/// </exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		XMPDateTime GetPropertyDate(string schemaNS, string propName);

		/// <summary>Convenience method to retrieve the literal value of a property.</summary>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <returns>
		/// Returns a Java <code>Calendar</code>-object or <code>null</code>
		/// if the property does not exist.
		/// </returns>
		/// <exception cref="XMPException">
		/// Wraps all exceptions that may occur,
		/// especially conversion errors.
		/// </exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		Sharpen.Calendar GetPropertyCalendar(string schemaNS, string propName);

		/// <summary>Convenience method to retrieve the literal value of a property.</summary>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <returns>
		/// Returns a <code>byte[]</code>-array contained the decoded base64 value
		/// or <code>null</code> if the property does not exist.
		/// </returns>
		/// <exception cref="XMPException">
		/// Wraps all exceptions that may occur,
		/// especially conversion errors.
		/// </exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		sbyte[] GetPropertyBase64(string schemaNS, string propName);

		/// <summary>Convenience method to retrieve the literal value of a property.</summary>
		/// <remarks>
		/// Convenience method to retrieve the literal value of a property.
		/// <em>Note:</em> There is no <code>setPropertyString()</code>,
		/// because <code>SetProperty()</code> sets a string value.
		/// </remarks>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>GetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <returns>
		/// Returns a <code>String</code> value or <code>null</code>
		/// if the property does not exist.
		/// </returns>
		/// <exception cref="XMPException">
		/// Wraps all exceptions that may occur,
		/// especially conversion errors.
		/// </exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		string GetPropertyString(string schemaNS, string propName);

		/// <summary>Convenience method to set a property to a literal <code>boolean</code> value.</summary>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>SetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <param name="propValue">the literal property value as <code>boolean</code>.</param>
		/// <param name="options">options of the property to set (optional).</param>
		/// <exception cref="XMPException">Wraps all exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetPropertyBoolean(string schemaNS, string propName, bool propValue, PropertyOptions options);

		/// <seealso cref="SetPropertyBoolean(string, string, bool, Com.Adobe.Xmp.Options.PropertyOptions)"/>
		/// <param name="schemaNS">The namespace URI for the property</param>
		/// <param name="propName">The name of the property</param>
		/// <param name="propValue">the literal property value as <code>boolean</code></param>
		/// <exception cref="XMPException">Wraps all exceptions</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetPropertyBoolean(string schemaNS, string propName, bool propValue);

		/// <summary>Convenience method to set a property to a literal <code>int</code> value.</summary>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>SetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <param name="propValue">the literal property value as <code>int</code>.</param>
		/// <param name="options">options of the property to set (optional).</param>
		/// <exception cref="XMPException">Wraps all exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetPropertyInteger(string schemaNS, string propName, int propValue, PropertyOptions options);

		/// <seealso cref="SetPropertyInteger(string, string, int, Com.Adobe.Xmp.Options.PropertyOptions)"/>
		/// <param name="schemaNS">The namespace URI for the property</param>
		/// <param name="propName">The name of the property</param>
		/// <param name="propValue">the literal property value as <code>int</code></param>
		/// <exception cref="XMPException">Wraps all exceptions</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetPropertyInteger(string schemaNS, string propName, int propValue);

		/// <summary>Convenience method to set a property to a literal <code>long</code> value.</summary>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>SetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <param name="propValue">the literal property value as <code>long</code>.</param>
		/// <param name="options">options of the property to set (optional).</param>
		/// <exception cref="XMPException">Wraps all exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetPropertyLong(string schemaNS, string propName, long propValue, PropertyOptions options);

		/// <seealso cref="SetPropertyLong(string, string, long, Com.Adobe.Xmp.Options.PropertyOptions)"/>
		/// <param name="schemaNS">The namespace URI for the property</param>
		/// <param name="propName">The name of the property</param>
		/// <param name="propValue">the literal property value as <code>long</code></param>
		/// <exception cref="XMPException">Wraps all exceptions</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetPropertyLong(string schemaNS, string propName, long propValue);

		/// <summary>Convenience method to set a property to a literal <code>double</code> value.</summary>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>SetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <param name="propValue">the literal property value as <code>double</code>.</param>
		/// <param name="options">options of the property to set (optional).</param>
		/// <exception cref="XMPException">Wraps all exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetPropertyDouble(string schemaNS, string propName, double propValue, PropertyOptions options);

		/// <seealso cref="SetPropertyDouble(string, string, double, Com.Adobe.Xmp.Options.PropertyOptions)"/>
		/// <param name="schemaNS">The namespace URI for the property</param>
		/// <param name="propName">The name of the property</param>
		/// <param name="propValue">the literal property value as <code>double</code></param>
		/// <exception cref="XMPException">Wraps all exceptions</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetPropertyDouble(string schemaNS, string propName, double propValue);

		/// <summary>
		/// Convenience method to set a property with an XMPDateTime-object,
		/// which is serialized to an ISO8601 date.
		/// </summary>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>SetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <param name="propValue">the property value as <code>XMPDateTime</code>.</param>
		/// <param name="options">options of the property to set (optional).</param>
		/// <exception cref="XMPException">Wraps all exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetPropertyDate(string schemaNS, string propName, XMPDateTime propValue, PropertyOptions options);

		/// <seealso cref="SetPropertyDate(string, string, XMPDateTime, Com.Adobe.Xmp.Options.PropertyOptions)"/>
		/// <param name="schemaNS">The namespace URI for the property</param>
		/// <param name="propName">The name of the property</param>
		/// <param name="propValue">the property value as <code>XMPDateTime</code></param>
		/// <exception cref="XMPException">Wraps all exceptions</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetPropertyDate(string schemaNS, string propName, XMPDateTime propValue);

		/// <summary>
		/// Convenience method to set a property with a Java Calendar-object,
		/// which is serialized to an ISO8601 date.
		/// </summary>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>SetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <param name="propValue">the property value as Java <code>Calendar</code>.</param>
		/// <param name="options">options of the property to set (optional).</param>
		/// <exception cref="XMPException">Wraps all exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetPropertyCalendar(string schemaNS, string propName, Sharpen.Calendar propValue, PropertyOptions options);

		/// <seealso cref="SetPropertyCalendar(string, string, Sharpen.Calendar, Com.Adobe.Xmp.Options.PropertyOptions)"/>
		/// <param name="schemaNS">The namespace URI for the property</param>
		/// <param name="propName">The name of the property</param>
		/// <param name="propValue">the property value as <code>Calendar</code></param>
		/// <exception cref="XMPException">Wraps all exceptions</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetPropertyCalendar(string schemaNS, string propName, Sharpen.Calendar propValue);

		/// <summary>
		/// Convenience method to set a property from a binary <code>byte[]</code>-array,
		/// which is serialized as base64-string.
		/// </summary>
		/// <param name="schemaNS">
		/// The namespace URI for the property. Has the same usage as in
		/// <code>SetProperty()</code>.
		/// </param>
		/// <param name="propName">
		/// The name of the property.
		/// Has the same usage as in <code>GetProperty()</code>.
		/// </param>
		/// <param name="propValue">the literal property value as byte array.</param>
		/// <param name="options">options of the property to set (optional).</param>
		/// <exception cref="XMPException">Wraps all exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetPropertyBase64(string schemaNS, string propName, sbyte[] propValue, PropertyOptions options);

		/// <seealso cref="SetPropertyBase64(string, string, sbyte[], Com.Adobe.Xmp.Options.PropertyOptions)"/>
		/// <param name="schemaNS">The namespace URI for the property</param>
		/// <param name="propName">The name of the property</param>
		/// <param name="propValue">the literal property value as byte array</param>
		/// <exception cref="XMPException">Wraps all exceptions</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void SetPropertyBase64(string schemaNS, string propName, sbyte[] propValue);

		/// <summary>Constructs an iterator for the properties within this XMP object.</summary>
		/// <returns>Returns an <code>XMPIterator</code>.</returns>
		/// <seealso cref="Iterator(string, string, Com.Adobe.Xmp.Options.IteratorOptions)"/>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		XMPIterator Iterator();

		/// <summary>Constructs an iterator for the properties within this XMP object using some options.</summary>
		/// <param name="options">Option flags to control the iteration.</param>
		/// <returns>Returns an <code>XMPIterator</code>.</returns>
		/// <seealso cref="Iterator(string, string, Com.Adobe.Xmp.Options.IteratorOptions)"/>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		XMPIterator Iterator(IteratorOptions options);

		/// <summary>Construct an iterator for the properties within an XMP object.</summary>
		/// <remarks>
		/// Construct an iterator for the properties within an XMP object. According to the parameters it iterates the entire data tree,
		/// properties within a specific schema, or a subtree rooted at a specific node.
		/// </remarks>
		/// <param name="schemaNS">
		/// Optional schema namespace URI to restrict the iteration. Omitted (visit all
		/// schema) by passing <code>null</code> or empty String.
		/// </param>
		/// <param name="propName">
		/// Optional property name to restrict the iteration. May be an arbitrary path
		/// expression. Omitted (visit all properties) by passing <code>null</code> or empty
		/// String. If no schema URI is given, it is ignored.
		/// </param>
		/// <param name="options">
		/// Option flags to control the iteration. See
		/// <see cref="Com.Adobe.Xmp.Options.IteratorOptions"/>
		/// for
		/// details.
		/// </param>
		/// <returns>
		/// Returns an <code>XMPIterator</code> for this <code>XMPMeta</code>-object
		/// considering the given options.
		/// </returns>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		XMPIterator Iterator(string schemaNS, string propName, IteratorOptions options);

		/// <summary>
		/// This correlates to the about-attribute,
		/// returns the empty String if no name is set.
		/// </summary>
		/// <returns>Returns the name of the XMP object.</returns>
		string GetObjectName();

		/// <param name="name">Sets the name of the XMP object.</param>
		void SetObjectName(string name);

		/// <returns>
		/// Returns the unparsed content of the &lt;?xpacket&gt; processing instruction.
		/// This contains normally the attribute-like elements 'begin="&lt;BOM&gt;"
		/// id="W5M0MpCehiHzreSzNTczkc9d"' and possibly the deprecated elements 'bytes="1234"' or
		/// 'encoding="XXX"'. If the parsed packet has not been wrapped into an xpacket,
		/// <code>null</code> is returned.
		/// </returns>
		string GetPacketHeader();

//		/// <summary>Clones the complete metadata tree.</summary>
//		/// <returns>Returns a deep copy of this instance.</returns>
//		object Clone();

		/// <summary>
		/// Sorts the complete datamodel according to the following rules:
		/// <ul>
		/// <li>Schema nodes are sorted by prefix.
		/// </summary>
		/// <remarks>
		/// Sorts the complete datamodel according to the following rules:
		/// <ul>
		/// <li>Schema nodes are sorted by prefix.
		/// <li>Properties at top level and within structs are sorted by full name, that is
		/// prefix + local name.
		/// <li>Array items are not sorted, even if they have no certain order such as bags.
		/// <li>Qualifier are sorted, with the exception of "xml:lang" and/or "rdf:type"
		/// that stay at the top of the list in that order.
		/// </ul>
		/// </remarks>
		void Sort();

		/// <summary>Perform the normalization as a separate parsing step.</summary>
		/// <remarks>
		/// Perform the normalization as a separate parsing step.
		/// Normally it is done during parsing, unless the parsing option
		/// <see cref="Com.Adobe.Xmp.Options.ParseOptions.OmitNormalization"/>
		/// is set to <code>true</code>.
		/// <em>Note:</em> It does no harm to call this method to an already normalized xmp object.
		/// It was a PDF/A requirement to get hand on the unnormalized <code>XMPMeta</code> object.
		/// </remarks>
		/// <param name="options">optional parsing options.</param>
		/// <exception cref="XMPException">Wraps all errors and exceptions that may occur.</exception>
		/// <exception cref="Com.Adobe.Xmp.XMPException"/>
		void Normalize(ParseOptions options);

		/// <summary>Renders this node and the tree unter this node in a human readable form.</summary>
		/// <returns>Returns a multiline string containing the dump.</returns>
		string DumpObject();
	}
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ISIS.GME.Dsml.ValueFlow.Interfaces
{
	
	
	/// <summary>
	///<para>This API code is compatible with the following paradigm:</para>
	///<para> - Name: ValueFlow</para>
	///<para> - Guid: {B7BEF8AB-BEEF-4369-9D6D-A7634868AC59}</para>
	///<para>Additional information: </para>
	///<para> - Date: 12/15/2016 3:30:25 PM</para>
	///<para> - Author: </para>
	///<para> - Version: </para>
	///<para> - Comment: </para>
	///</summary>
	///
	/// <summary>
	/// <para>RootFolder interface</para>
	/// </summary>
	public interface RootFolder : ISIS.GME.Common.Interfaces.RootFolder
	{
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.RootFolder.InfoClass Info
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.Dictionary<int, global::System.Type> MetaRefs
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the library's connection string if this object is an attached library.</para>
		///<para></para>
		///</summary>
		string LibraryName
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific attached libraries.</para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Dsml.ValueFlow.Interfaces.RootFolder> LibraryCollection
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific child objects.</para>
		///<para></para>
		///</summary>
		Classes.RootFolder.ChildrenClass Children
		{
			get;
		}
		
		/// <summary>
		///<para>Contains all the domain specific child objects.</para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Base> AllChildren
		{
			get;
		}
	}
	
	/// <summary>
	/// <para>ValueFlow interface</para>
	/// </summary>
	public interface ValueFlow : ISIS.GME.Common.Interfaces.Connection
	{
		
		/// <summary>
		///<para>Contains the domain specific source end point of this connection.</para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllSrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a DESTINATION.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.ValueFlow.SrcConnectionsClass SrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific destination end point of this connection.</para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllDstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a SOURCE.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.ValueFlow.DstConnectionsClass DstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific source end point of this connection.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Common.Interfaces.FCO SrcEnd
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific source end point of this connection.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.ValueFlow.SrcEndsClass SrcEnds
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific destination end point of this connection.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Common.Interfaces.FCO DstEnd
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific destination end point of this connection.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.ValueFlow.DstEndsClass DstEnds
		{
			get;
		}
		
		/// <summary>
		///<para>the object that is at the farthest position within the chain of base objects (i.e. the one which is not derived from anything).</para>
		///<para> NULL if the object is not derived.</para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Interfaces.ValueFlow ArcheType
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific attributes.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.ValueFlow.AttributesClass Attributes
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.ValueFlow.ReferencedByClass ReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllMembersOfSet
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.ValueFlow.MembersOfSetClass MembersOfSet
		{
			get;
		}
	}
	
	/// <summary>
	/// <para>Component interface</para>
	/// </summary>
	public interface Component : ISIS.GME.Common.Interfaces.Model
	{
		
		/// <summary>
		///<para>Contains the domain specific source end point of this connection.</para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllSrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a DESTINATION.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.Component.SrcConnectionsClass SrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific destination end point of this connection.</para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllDstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a SOURCE.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.Component.DstConnectionsClass DstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>the object that is at the farthest position within the chain of base objects (i.e. the one which is not derived from anything).</para>
		///<para> NULL if the object is not derived.</para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Interfaces.Component ArcheType
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific attributes.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.Component.AttributesClass Attributes
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific child objects.</para>
		///<para></para>
		///</summary>
		Classes.Component.ChildrenClass Children
		{
			get;
		}
		
		/// <summary>
		///<para>Contains all the domain specific child objects.</para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Base> AllChildren
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.Component.ReferencedByClass ReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllMembersOfSet
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.Component.MembersOfSetClass MembersOfSet
		{
			get;
		}
	}
	
	/// <summary>
	/// <para>Parameter interface</para>
	/// <para>-----------------------------------------------</para>
	/// <para>Base types:</para>
	/// <para>- NamedElement</para>
	/// </summary>
	public interface Parameter : ISIS.GME.Common.Interfaces.Atom, ISIS.GME.Dsml.ValueFlow.Interfaces.NamedElement
	{
		
		/// <summary>
		///<para>Contains the domain specific source end point of this connection.</para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllSrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a DESTINATION.</para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.Parameter.SrcConnectionsClass SrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific destination end point of this connection.</para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllDstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a SOURCE.</para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.Parameter.DstConnectionsClass DstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>the object that is at the farthest position within the chain of base objects (i.e. the one which is not derived from anything).</para>
		///<para> NULL if the object is not derived.</para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Interfaces.Parameter ArcheType
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific attributes.</para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.Parameter.AttributesClass Attributes
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.Parameter.ReferencedByClass ReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllMembersOfSet
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.Parameter.MembersOfSetClass MembersOfSet
		{
			get;
		}
	}
	
	/// <summary>
	/// <para>Output interface</para>
	/// <para>-----------------------------------------------</para>
	/// <para>Base types:</para>
	/// <para>- NamedElement</para>
	/// </summary>
	public interface Output : ISIS.GME.Common.Interfaces.Atom, ISIS.GME.Dsml.ValueFlow.Interfaces.NamedElement
	{
		
		/// <summary>
		///<para>Contains the domain specific source end point of this connection.</para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllSrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a DESTINATION.</para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.Output.SrcConnectionsClass SrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific destination end point of this connection.</para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllDstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a SOURCE.</para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.Output.DstConnectionsClass DstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>the object that is at the farthest position within the chain of base objects (i.e. the one which is not derived from anything).</para>
		///<para> NULL if the object is not derived.</para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Interfaces.Output ArcheType
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific attributes.</para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.Output.AttributesClass Attributes
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.Output.ReferencedByClass ReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllMembersOfSet
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.Output.MembersOfSetClass MembersOfSet
		{
			get;
		}
	}
	
	/// <summary>
	/// <para>ComplexFormula interface</para>
	/// <para>-----------------------------------------------</para>
	/// <para>Base types:</para>
	/// <para>- FormulaAtom</para>
	/// </summary>
	public interface ComplexFormula : ISIS.GME.Common.Interfaces.Atom, ISIS.GME.Dsml.ValueFlow.Interfaces.FormulaAtom
	{
		
		/// <summary>
		///<para>Contains the domain specific source end point of this connection.</para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllSrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a DESTINATION.</para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.ComplexFormula.SrcConnectionsClass SrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific destination end point of this connection.</para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllDstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a SOURCE.</para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.ComplexFormula.DstConnectionsClass DstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>the object that is at the farthest position within the chain of base objects (i.e. the one which is not derived from anything).</para>
		///<para> NULL if the object is not derived.</para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Interfaces.ComplexFormula ArcheType
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific attributes.</para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.ComplexFormula.AttributesClass Attributes
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.ComplexFormula.ReferencedByClass ReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllMembersOfSet
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.ComplexFormula.MembersOfSetClass MembersOfSet
		{
			get;
		}
	}
	
	/// <summary>
	/// <para>SimpleFormula interface</para>
	/// <para>-----------------------------------------------</para>
	/// <para>Base types:</para>
	/// <para>- FormulaAtom</para>
	/// </summary>
	public interface SimpleFormula : ISIS.GME.Common.Interfaces.Atom, ISIS.GME.Dsml.ValueFlow.Interfaces.FormulaAtom
	{
		
		/// <summary>
		///<para>Contains the domain specific source end point of this connection.</para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllSrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a DESTINATION.</para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.SimpleFormula.SrcConnectionsClass SrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific destination end point of this connection.</para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllDstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a SOURCE.</para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.SimpleFormula.DstConnectionsClass DstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>the object that is at the farthest position within the chain of base objects (i.e. the one which is not derived from anything).</para>
		///<para> NULL if the object is not derived.</para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Interfaces.SimpleFormula ArcheType
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific attributes.</para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.SimpleFormula.AttributesClass Attributes
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.SimpleFormula.ReferencedByClass ReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllMembersOfSet
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.SimpleFormula.MembersOfSetClass MembersOfSet
		{
			get;
		}
	}
	
	/// <summary>
	/// <para>Python interface</para>
	/// </summary>
	public interface Python : ISIS.GME.Common.Interfaces.Model
	{
		
		/// <summary>
		///<para>Contains the domain specific source end point of this connection.</para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllSrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a DESTINATION.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.Python.SrcConnectionsClass SrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific destination end point of this connection.</para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllDstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a SOURCE.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.Python.DstConnectionsClass DstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>the object that is at the farthest position within the chain of base objects (i.e. the one which is not derived from anything).</para>
		///<para> NULL if the object is not derived.</para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Interfaces.Python ArcheType
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific attributes.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.Python.AttributesClass Attributes
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific child objects.</para>
		///<para></para>
		///</summary>
		Classes.Python.ChildrenClass Children
		{
			get;
		}
		
		/// <summary>
		///<para>Contains all the domain specific child objects.</para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Base> AllChildren
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.Python.ReferencedByClass ReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllMembersOfSet
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.Python.MembersOfSetClass MembersOfSet
		{
			get;
		}
	}
	
	/// <summary>
	/// <para>FormulaAtom interface</para>
	/// </summary>
	public interface FormulaAtom : ISIS.GME.Common.Interfaces.Atom
	{
		
		/// <summary>
		///<para>Contains the domain specific source end point of this connection.</para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllSrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a DESTINATION.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.FormulaAtom.SrcConnectionsClass SrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific destination end point of this connection.</para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllDstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a SOURCE.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.FormulaAtom.DstConnectionsClass DstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>the object that is at the farthest position within the chain of base objects (i.e. the one which is not derived from anything).</para>
		///<para> NULL if the object is not derived.</para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Interfaces.FormulaAtom ArcheType
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific attributes.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.FormulaAtom.AttributesClass Attributes
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.FormulaAtom.ReferencedByClass ReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllMembersOfSet
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.FormulaAtom.MembersOfSetClass MembersOfSet
		{
			get;
		}
	}
	
	/// <summary>
	/// <para>Input interface</para>
	/// <para>-----------------------------------------------</para>
	/// <para>Base types:</para>
	/// <para>- NamedElement</para>
	/// </summary>
	public interface Input : ISIS.GME.Common.Interfaces.Atom, ISIS.GME.Dsml.ValueFlow.Interfaces.NamedElement
	{
		
		/// <summary>
		///<para>Contains the domain specific source end point of this connection.</para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllSrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a DESTINATION.</para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.Input.SrcConnectionsClass SrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific destination end point of this connection.</para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllDstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a SOURCE.</para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.Input.DstConnectionsClass DstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>the object that is at the farthest position within the chain of base objects (i.e. the one which is not derived from anything).</para>
		///<para> NULL if the object is not derived.</para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Interfaces.Input ArcheType
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific attributes.</para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.Input.AttributesClass Attributes
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.Input.ReferencedByClass ReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllMembersOfSet
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		new ISIS.GME.Dsml.ValueFlow.Classes.Input.MembersOfSetClass MembersOfSet
		{
			get;
		}
	}
	
	/// <summary>
	/// <para>NamedElement interface</para>
	/// </summary>
	public interface NamedElement : ISIS.GME.Common.Interfaces.Atom
	{
		
		/// <summary>
		///<para>Contains the domain specific source end point of this connection.</para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllSrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a DESTINATION.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.NamedElement.SrcConnectionsClass SrcConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific destination end point of this connection.</para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.Connection> AllDstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>Retrieves all connections, which have this object as a SOURCE.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.NamedElement.DstConnectionsClass DstConnections
		{
			get;
		}
		
		/// <summary>
		///<para>the object that is at the farthest position within the chain of base objects (i.e. the one which is not derived from anything).</para>
		///<para> NULL if the object is not derived.</para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Interfaces.NamedElement ArcheType
		{
			get;
		}
		
		/// <summary>
		///<para>Contains the domain specific attributes.</para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.NamedElement.AttributesClass Attributes
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.NamedElement.ReferencedByClass ReferencedBy
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		global::System.Collections.Generic.IEnumerable<ISIS.GME.Common.Interfaces.FCO> AllMembersOfSet
		{
			get;
		}
		
		/// <summary>
		///<para></para>
		///<para></para>
		///</summary>
		ISIS.GME.Dsml.ValueFlow.Classes.NamedElement.MembersOfSetClass MembersOfSet
		{
			get;
		}
	}
}

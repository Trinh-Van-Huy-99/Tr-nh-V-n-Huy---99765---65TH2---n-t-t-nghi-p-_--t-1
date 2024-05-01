

using System;
using System.Collections.Generic;
using System.Text;
//using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using DATN_TRINHVANHUY_99765_65TH2.Forms;
using DATN_TRINHVANHUY_99765_65TH2.OOP;
using System.Windows.Controls;


namespace DATN_TRINHVANHUY_99765_65TH2
{
   /// <summary>
   /// The class derived form FramReinMaker showes how to create the rebars for a column
   /// </summary>
   public class ColumnFramReinMaker : FramReinMaker
   {
      #region Private Members

      ColumnGeometrySupport m_geometry; // The geometry support for column rebar creation

      RebarBarType m_transverseEndType = Cls_BienChuongTrinh.cls_ThepDai.Bartypethepdai;      //type of the end transverse rebar
      RebarBarType m_transverseCenterType = Cls_BienChuongTrinh.cls_ThepDai.Bartypethepdai;   //type of the center transverse rebar
        RebarBarType m_verticalType = Cls_BienChuongTrinh.cls_ThepDoc.Bartypethepdoc;          //type of the vertical rebar
        RebarHookType m_transverseHookType = Cls_BienChuongTrinh.cls_ThepDai.Hookatstart;    //type of the hook

      double m_transverseEndSpacing = Cls_BienChuongTrinh.cls_ThepDai.S1;    //the space value of end transverse rebar
      double m_transverseCenterSpacing = Cls_BienChuongTrinh.cls_ThepDai.S2; //the space value of center transverse rebar
        int m_verticalRebarNumber = Cls_BienChuongTrinh.cls_ThepDoc.Soluongphuongz;        //the number of the vertical rebar

      #endregion

      #region Properties

      /// <summary>
      /// get and set the type of the end transverse rebar
      /// </summary>
      public RebarBarType TransverseEndType
      {
         get
         {
            return m_transverseEndType;
         }
         set
         {
            m_transverseEndType = value;
         }
      }

      /// <summary>
      /// get and set the type of the center transverse rebar
      /// </summary>
      public RebarBarType TransverseCenterType
      {
         get
         {
            return m_transverseCenterType;
         }
         set
         {
            m_transverseCenterType = value;
         }
      }

      /// <summary>
      /// get and set the type of the vertical rebar
      /// </summary>
      public RebarBarType VerticalRebarType
      {
         get
         {
            return m_verticalType;
         }
         set
         {
            m_verticalType = value;
         }
      }

      /// <summary>
      /// get and set the space value of end transverse rebar
      /// </summary>
      public double TransverseEndSpacing
      {
         get
         {
            return m_transverseEndSpacing;
         }
         set
         {
            if (0 > value)  // spacing data must be above 0
            {
               throw new Exception("Transverse end spacing should be above zero");
            }
            m_transverseEndSpacing = value;
         }
      }

      /// <summary>
      /// get and set the space value of center transverse rebar
      /// </summary>
      public double TransverseCenterSpacing
      {
         get
         {
            return m_transverseCenterSpacing;
         }
         set
         {
            if (0 > value)  // spacing data must be above 0
            {
               throw new Exception("Transverse center spacing should be above zero");
            }
            m_transverseCenterSpacing = value;
         }
      }

      /// <summary>
      /// get and set the number of vertical rebar
      /// </summary>
      public int VerticalRebarNumber
      {
         get
         {
            return m_verticalRebarNumber;
         }
         set
         {
            if (4 > value)  // vertical rebar number must be above 3
            {
               throw new Exception("The minimum of vertical rebar number shouble be four.");
            }
            m_verticalRebarNumber = value;
         }
      }

      /// <summary>
      /// get and set the hook type of transverse rebar
      /// </summary>
      public RebarHookType TransverseHookType
      {
         get
         {
            return m_transverseHookType;
         }
         set
         {
            m_transverseHookType = value;
         }
      }

      #endregion

      #region Constructor
      /// <summary>
      /// Constructor of the ColumnFramReinMaker
      /// </summary>
      /// <param name="commandData">the ExternalCommandData reference</param>
      /// <param name="hostObject">the host column</param>
      public ColumnFramReinMaker(ExternalCommandData commandData, FamilyInstance hostObject)
         : base(commandData, hostObject)
      {
         //create a new options for current project
         Options geoOptions = commandData.Application.Application.Create.NewGeometryOptions();
         geoOptions.ComputeReferences = true;

         //create a ColumnGeometrySupport instance 
         m_geometry = new ColumnGeometrySupport(hostObject, geoOptions);
      }
      #endregion

      #region Override Methods


      protected override bool AssertData()
      {
         return base.AssertData();
      }

      protected override bool DisplayForm()
      {
            ViewLoadData frm_loaddata = new ViewLoadData(this);
            bool? _result = frm_loaddata.ShowDialog();
            if (_result == true)
            {
                return true;
            }
            return base.DisplayForm();
      }
      /// <summary>
      /// Override method to create rebars on the selected column
      /// </summary>
      /// <returns>true if the creation is successful, otherwise false.</returns>
      protected override bool FillWithBars()
      {
         // create the transverse rebars
         bool flag = FillTransverseBars();

         // create the vertical rebars
         flag = flag && FillVerticalBars();

         return base.FillWithBars();
      }

      #endregion

      /// <summary>
      /// create the transverse rebars for the column
      /// </summary>
      /// <returns>true if the creation is successful, otherwise false</returns>
      public bool FillTransverseBars()
      {
         // create all kinds of transverse rebars according to the TransverseRebarLocation
         foreach (TransverseRebarLocation location in Enum.GetValues(
                                                     typeof(TransverseRebarLocation)))
         {
            Rebar createdRebar = FillTransverseBar(location);
            //judge whether the transverse rebar creation is successful
            if (null == createdRebar)
            {
               return false;
            }
         }

         return true;
      }


      public Rebar FillTransverseBar(TransverseRebarLocation location)
      {
         // Get the geometry information which support rebar creation
         RebarGeometry geomInfo = new RebarGeometry();
         RebarBarType barType = null;
         switch (location)
         {
            case TransverseRebarLocation.Start: // start transverse rebar
            case TransverseRebarLocation.End:   // end transverse rebar
               geomInfo = m_geometry.GetTransverseRebar(location, m_transverseEndSpacing);
               barType = m_transverseEndType;
               break;
            case TransverseRebarLocation.Center:// center transverse rebar   
               geomInfo = m_geometry.GetTransverseRebar(location, m_transverseCenterSpacing);
               barType = m_transverseCenterType;
               break;
            default:
               break;
         }

         // create the rebar
         return PlaceRebars(barType, m_transverseHookType, m_transverseHookType,
                                         geomInfo, RebarHookOrientation.Right, RebarHookOrientation.Left);
      }


      /// <summary>
      /// Create the vertical rebar according the location
      /// </summary>
      /// <param name="location">location of rebar which need to be created</param>
      /// <returns>the created rebar, return null if the creation is unsuccessful</returns>
      public Rebar FillVerticalBar(VerticalRebarLocation location)
      {
         //calculate the rebar number in different location
         int rebarNubmer = m_verticalRebarNumber / 4;
         switch (location)
         {
            case VerticalRebarLocation.East:    // the east vertical rebar
               if (0 < m_verticalRebarNumber % 4)
               {
                  rebarNubmer++;
               }
               break;
            case VerticalRebarLocation.North:   // the north vertical rebar
               if (2 < m_verticalRebarNumber % 4)
               {
                  rebarNubmer++;
               }
               break;
            case VerticalRebarLocation.West:    // the west vertical rebar
               if (1 < m_verticalRebarNumber % 4)
               {
                  rebarNubmer++;
               }
               break;
            case VerticalRebarLocation.South:   // the south vertical rebar
               break;
         }

         // get the geometry information for rebar creation
         RebarGeometry geomInfo = m_geometry.GetVerticalRebar(location, rebarNubmer);

         // create the rebar
         return PlaceRebars(m_verticalType, null, null, geomInfo,
                                     RebarHookOrientation.Left, RebarHookOrientation.Left);
      }


      /// <summary>
      /// create the all the vertial rebar
      /// </summary>
      /// <returns>true if the creation is successful, otherwise false</returns>
      private bool FillVerticalBars()
      {
         // create all kinds of vertical rebars according to the VerticalRebarLocation
         foreach (VerticalRebarLocation location in Enum.GetValues(
                                             typeof(VerticalRebarLocation)))
         {
            Rebar createdRebar = FillVerticalBar(location);
            //judge whether the vertical rebar creation is successful
            if (null == createdRebar)
            {
               return false;
            }
         }

         return true;
      }
   }
}

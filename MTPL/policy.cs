//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MTPL
{
    using System;
    using System.Collections.Generic;
    
    public partial class policy
    {
        public int policy_id { get; set; }
        public string policy_number { get; set; }
        public string insurance_company { get; set; }
        public System.DateTime issue_date { get; set; }
        public System.DateTime expiration_date { get; set; }
        public int driver_id { get; set; }
        public int car_id { get; set; }
        public string driving_license_series { get; set; }
        public string driving_license_number { get; set; }
        public decimal cost { get; set; }
    
        public virtual car car { get; set; }
        public virtual driver driver { get; set; }
    }
}

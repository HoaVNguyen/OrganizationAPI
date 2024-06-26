﻿using Organization.Features.OfficeFeatures.DTO;

namespace Organization.Features.ParishFeatures
{
    public class ParishDto
    {
        public int ParishId { get; set; }
        public string Name { get; set; }
        public int ZipCode { get; set; }
        public OfficeForCreationDto? Office { get; set; }
    }

    public class ParishForCreationDto
    {
        public string Name { get; set; }
        public int ZipCode { get; set; }

        public int officeId { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace FaceSystem.Tool
{
    public interface IDocumentRenderer
    {
        void Render(FlowDocument doc, Object data);
    }
}

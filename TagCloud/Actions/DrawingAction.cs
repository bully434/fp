﻿using TagCloud.ExceptionHandler;
using TagCloud.Forms;
using TagCloud.Interfaces;
using TagCloud.Settings;
using TagCloud.TagCloudVisualization.Visualization;
using TagCloud.Words;

namespace TagCloud.Actions
{
    public class DrawingAction : IUiAction
    {
        private readonly IExceptionHandler exceptionHandler;
        private readonly ImageBox imageBox;
        private readonly ITagGenerator tagGenerator;
        private readonly IWordRepository wordsRepository;
        private ImageSettings imageSettings;

        public DrawingAction(ImageBox imageBox, IWordRepository wordsRepository,
            ITagGenerator tagGenerator, ImageSettings imageSettings,
            IExceptionHandler exceptionHandler)
        {
            this.imageBox = imageBox;
            this.wordsRepository = wordsRepository;
            this.tagGenerator = tagGenerator;
            this.exceptionHandler = exceptionHandler;
            this.imageSettings = imageSettings;
        }

        public string Category => "Tag Cloud";
        public string Name => "Draw";
        public string Description => "Draw new Tag Cloud";

        public void Perform()
        {
            imageSettings.UpdateSettings();
            tagGenerator.GetTags(wordsRepository.Get())
                .Then(tags => new TagCloudVisualizer(imageBox, imageSettings, tags))
                .Then(tagCloud => tagCloud.GetTagCloudImage())
                .RefineError("Failed, trying to get a Tag Cloud image")
                .OnFail(exceptionHandler.HandleException);
        }
    }
}